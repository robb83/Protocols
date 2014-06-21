using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Protocol.ProtoBuffers.Serializer;
using Protocol.ProtoBuffers;

namespace Protocol.ProtoBuffers
{
    public static class Serializers
    {
        public static T Deserialize<T>(Stream source)
        {
            return (T)Deserialize(source, typeof(T));
        }

        public static object Deserialize(Stream source, Type expectedType)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (expectedType == null)
                throw new ArgumentNullException("expectedType");

            IProtoSerializer serializer = null;

            serializer = TypeDescriptions.GetDescriptionForType(expectedType).MessageSerializer;

            using(ProtoReader reader = new ProtoReader(source))
            {
                // ha nincs üzenet, akkor az null
                if (reader.ReadMessageHeader() == 0)
                {
                    return null;
                }

                return serializer.Read(reader);
            }
        }

        public static void Serialize(Stream destination, object graph)
        {
            if (destination == null)
                throw new ArgumentNullException("destination");

            IProtoSerializer serializer = null;
            
            if (graph != null)
            {
                serializer = TypeDescriptions.GetDescriptionForType(graph.GetType()).MessageSerializer;
            }

            using (ProtoWriter writer = new ProtoWriter(destination))
            {
                if (serializer != null)
                {
                    serializer.Writer(writer, graph);
                }
            }
        }

    }

    public static class TypeDescriptions
    {
        static object writeLock = new object();
        public static volatile Dictionary<Type, TypeDescription> types = new Dictionary<Type, TypeDescription>();
        static Dictionary<Type, TypeDescription> secondaryTypes;

        static TypeDescriptions()
        {
            // primitives
            AddPrimitiveTypeDescription(typeof(Boolean), new BooleanSerializer());
            AddPrimitiveTypeDescription(typeof(Byte[]), new ByteArraySerializer());
            AddPrimitiveTypeDescription(typeof(Byte), new UInt8Serializer());
            AddPrimitiveTypeDescription(typeof(Double), new DoubleSerializer());
            AddPrimitiveTypeDescription(typeof(Decimal), new DecimalSerializer());
            AddPrimitiveTypeDescription(typeof(DateTime), new DateTimeSerializer());
            AddPrimitiveTypeDescription(typeof(Guid), new GuidSerializer());
            AddPrimitiveTypeDescription(typeof(Int16), new Int16Serializer());
            AddPrimitiveTypeDescription(typeof(Int32), new Int32Serializer());
            AddPrimitiveTypeDescription(typeof(Int64), new Int64Serializer());
            AddPrimitiveTypeDescription(typeof(String), new StringSerializer());
            AddPrimitiveTypeDescription(typeof(SByte), new Int8Serializer());
            AddPrimitiveTypeDescription(typeof(Single), new SingleSerializer());
            AddPrimitiveTypeDescription(typeof(TimeSpan), new TimeSpanSerializer());
            AddPrimitiveTypeDescription(typeof(UInt16), new UInt16Serializer());
            AddPrimitiveTypeDescription(typeof(UInt32), new UInt32Serializer());
            AddPrimitiveTypeDescription(typeof(UInt64), new UInt64Serializer());
            AddPrimitiveTypeDescription(typeof(Uri), new UriSerializer());
        }

        public static TypeDescription GetDescriptionForType(Type objectType)
        {
            TypeDescription description = null;

            if (types.TryGetValue(objectType, out description))
            {
                return description;
            }

            description = BuildDescriptionForType(objectType);

            if (description == null || !description.Supported)
            {
                throw new NotSupportedException();
            }

            return description;
        }

        static TypeDescription BuildDescriptionForType(Type objectType)
        {
            lock(writeLock)
            {
                TypeDescription description = null;
                Dictionary<Type, TypeDescription> t = types;

                // double checking
                if (t.TryGetValue(objectType, out description))
                {
                    return description;
                }
                
                secondaryTypes = new Dictionary<Type, TypeDescription>(t);

                description = InternalDescriptionForType(objectType);

                types = secondaryTypes;
                secondaryTypes = null;

                return description;
            }
        }

        static void AddPrimitiveTypeDescription(Type expectedType, IProtoSerializer serializer)
        {
            types.Add(expectedType, new TypeDescription(expectedType, new PrimitivesDecorator(serializer), serializer));
        }

        static TypeDescription InternalDescriptionForType(Type objectType)
        {
            Type elementType;
            TypeDescription elementTypeDescription;
            TypeDescription description = null;
            IProtoSerializer primitiveSerializer = null;
            IProtoSerializer messageSerializer = null;
            bool returnWithUncompleted = false;

            try
            {
                if (secondaryTypes.TryGetValue(objectType, out description))
                {
                    returnWithUncompleted = true;
                    return description;
                }

                // új típus bejegyzése, de még nem jelentjük készre
                description = new TypeDescription(objectType);
                secondaryTypes.Add(objectType, description);

                if (objectType.IsPrimitive)
                {
                    // ezek azok a primitív típusok amiket jelenleg nem támogatunk
                    // készíteni kell hozzá egy sorosítót, majd felvenni itt a statikus konstruktorba
                }
                else if (objectType.IsEnum)
                {
                    primitiveSerializer = new EnumSerializer(objectType);
                    messageSerializer = new PrimitivesDecorator(primitiveSerializer);
                }
                else if (objectType.IsArray)
                {
                    if (objectType.GetArrayRank() != 1)
                    {
                        // több dimenziós tömböket nem támogatjuk
                    }
                    else
                    {
                        elementType = objectType.GetElementType();

                        if (elementType == TypeHelper.byteType)
                        {
                            // ezt már ismernünk kellet volna
                            primitiveSerializer = new ByteArraySerializer();
                            messageSerializer = new PrimitivesDecorator(primitiveSerializer);
                        }
                        else
                        {
                            // minden egyéb tömböt listaként sorosítunk
                            elementTypeDescription = InternalDescriptionForType(elementType);
                            description.AddDepends(elementTypeDescription);

                            primitiveSerializer = new ListSerializer(true, elementType, null, elementTypeDescription);
                            messageSerializer = new PrimitivesDecorator(primitiveSerializer);
                        }
                    }
                }
                else
                {
                    if (TypeHelper.IsNullable(objectType, out elementType))
                    {
                        elementTypeDescription = InternalDescriptionForType(elementType);
                        description.AddDepends(elementTypeDescription);

                        primitiveSerializer = new NullableSerializer(elementTypeDescription);
                        messageSerializer = new PrimitivesDecorator(primitiveSerializer);
                    }
                    else if (TypeHelper.IsSupportedList(objectType, out elementType))
                    {
                        elementTypeDescription = InternalDescriptionForType(elementType);
                        description.AddDepends(elementTypeDescription);

                        primitiveSerializer = new ListSerializer(false, elementType, objectType, elementTypeDescription);
                        messageSerializer = new PrimitivesDecorator(primitiveSerializer);
                    }
                    else if (TypeHelper.IsSupportedCustomType(objectType))
                    {
                        messageSerializer = GenerateTypeSerializerForType(description, objectType);
                        primitiveSerializer = new TypeDecorator(messageSerializer);
                    }
                }

                return description;
            }
            finally
            {
                if (!returnWithUncompleted && description != null)
                {
                    description.Complete(messageSerializer, primitiveSerializer);
                }
            }
        }

        static TypeSerializer GenerateTypeSerializerForType(TypeDescription description, Type objectType)
        {
            int fn;
            bool ignored;
            var fields = objectType.GetFields();
            var properties = objectType.GetProperties();
            LinkedList<TypeSerializer.IMemberSerializer> serializer = new LinkedList<TypeSerializer.IMemberSerializer>();
            TypeDescription memberTypeDescription;

            foreach (var f in fields)
            {
                TypeHelper.SearchMemberMetaData(f, out fn, out ignored);

                if (ignored)
                    continue;

                if (f.IsStatic)
                    continue;

                memberTypeDescription = InternalDescriptionForType(f.FieldType);
                serializer.AddLast(new TypeSerializer.FieldSerializer(fn, f, memberTypeDescription));
                description.AddDepends(memberTypeDescription);
            }

            foreach (var p in properties)
            {
                TypeHelper.SearchMemberMetaData(p, out fn, out ignored);

                if (ignored)
                    continue;

                if (!(p.CanRead && p.CanWrite))
                    continue;

                if (p.GetIndexParameters().Length > 0)
                    continue;

                memberTypeDescription = InternalDescriptionForType(p.PropertyType);
                serializer.AddLast(new TypeSerializer.PropertySerializer(fn, p, memberTypeDescription));
                description.AddDepends(memberTypeDescription);
            }

            return new TypeSerializer(objectType, serializer);
        }
    }

    public static class TypeHelper
    {
        public static Type listType = typeof(IList);
        public static Type collectionType = typeof(ICollection);
        public static Type enumerableType = typeof(IEnumerable);
        public static Type dictionaryType = typeof(IDictionary);
        public static Type stringType = typeof(String);
        public static Type byteType = typeof(byte);

        public static bool IsNullable(Type objectType, out Type itemType)
        {
            itemType = Nullable.GetUnderlyingType(objectType);

            return itemType != null;
        }

        public static Type GetListElementType(Type objectType)
        {
            if (objectType == null)
            {
                return null;
            }

            Type[] genericTypes = objectType.GetGenericArguments();

            if (genericTypes == null || genericTypes.Length != 1)
                return null;

            return genericTypes[0];
        }

        public static bool IsSupportedList(Type objectType, out Type itemType)
        {
            if (objectType == null)
            {
                itemType = null;
                return false;
            }

            Type[] interfaces = objectType.GetInterfaces();

            if (interfaces == null)
            {
                itemType = null;
                return false;
            }

            foreach (var i in interfaces)
            {
                if (i == listType)
                {
                    itemType = GetListElementType(objectType);
                    return itemType != null;
                }
            }

            itemType = null;
            return false;
        }
        
        public static bool IsSupportedCustomType(Type objectType)
        {
            if (objectType == null)
                return false;

            if (objectType.IsAbstract)
                return false;

            if (objectType.IsGenericTypeDefinition)
                return false;

            object[] attributes = objectType.GetCustomAttributes(false);

            if (attributes == null)
                return false;
            
            foreach (var attr in attributes)
            {
                if (attr is ProtoContractAttribute)
                {
                    if (objectType.GetConstructor(new Type[0]) != null)
                        return true;

                    return false;
                }
            }

            return false;
        }
        
        public static void SearchMemberMetaData(MemberInfo memberInfo, out int fieldNumber, out bool ignored)
        {
            object[] attributes = memberInfo.GetCustomAttributes(false);

            fieldNumber = 0;
            ignored = true;

            if (attributes == null)
                return;

            foreach (var attr in attributes)
            {
                if (attr is ProtoIgnoreAttribute)
                {
                    ignored = true;
                    break;
                }
                else if (attr is ProtoMemberAttribute)
                {
                    ignored = false;
                    fieldNumber = ((ProtoMemberAttribute)attr).Tag;
                }
            }
        }
    }
}
