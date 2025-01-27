using DeepCopy.Attributes;
using DeepCopy.Enums;
using System.Collections;
using System.Reflection;

namespace DeepCopy.Services
{
    public class CopyService<T> where T : class
    {

        private Dictionary<object, object>? _copiedAndRealInstances { get; set; }
        private T _instance { get; set; }


        //initialize an instance and Dictionary 
        //Ctors
        public CopyService(T instance)
        {
            _instance = instance;
            _copiedAndRealInstances = new Dictionary<object, object>();
        }



        //EntryPointMethod
        public object GetCopy(object DeepObject = null)
        {

            //GetType From Object or DeepObject(Recursion)   
            Type ObjectType = DeepObject?.GetType() ??
                typeof(T);

            //Return Instance Copy if it Already been Created yet
            AlreadyExistingObjectHandler(DeepObject, out var InstanceCopy);
            if (InstanceCopy != null)
                return InstanceCopy;

            //if Copy of the Current Object is Already Exist


            //CtorArgs
            List<object> ConstructorArgs = new List<object>();

            var ObjectMembersInfo = GetMembersFromTypeOfProcessObject(ObjectType);

            foreach (var ObjectMemberInfo in ObjectMembersInfo)
            {
                var CopyAttribute = GetCopyAttribute(ObjectMemberInfo);

                var CopyEnum = GetCopyEnumFromAttribute(CopyAttribute);

                var CopyInstance =
                    (CopyEnum == CopyEnum.Shallow) ?
                    ShallowCopyHandler(DeepObject ?? _instance, ObjectMemberInfo) :
                    DeepCopyHandler(DeepObject ?? _instance, ObjectMemberInfo);


                AddArgumentInstanceInObjectCtorCollection(ObjectMemberInfo, CopyInstance, ConstructorArgs);
            }
            if (AlreadyExistingObjectHandler(_instance, out var objectCopy))
            {
                return objectCopy;
            }

            var ObjectCopyInstance = InstanceCreatingHandler(ConstructorArgs, ObjectType);

            SetCopiedObjectIntoDictionary(DeepObject, ObjectCopyInstance);


            return ObjectCopyInstance;
        }



        //HandlingMethods
        //Private Methods
        private void CircularReferenceHandler(object ClonObjectInstance)
        {

        }

        private object CollectionDeepCopyHandler(IList DeepObjectsEnumerable)
        {
            ArgumentNullException(DeepObjectsEnumerable);

            var EnumerbaleArgs = new List<object>();

            foreach (var DeepObjectEnumerable in DeepObjectsEnumerable)
            {
                EnumerbaleArgs.Add(GetCopy(DeepObjectEnumerable));
            }

            var Instance = Activator.CreateInstance(DeepObjectsEnumerable.GetType());

            foreach (var item in EnumerbaleArgs)
            {
                ((IList)Instance).Add(item);
            }
            return ((IList)Instance);
        }
        //AlreadyExist
        private bool AlreadyExistingObjectHandler(object Instance, out object instanceCopy)
        {
            ArgumentNullException(Instance);

            if (_copiedAndRealInstances.TryGetValue(Instance, out instanceCopy))
                return true;

            instanceCopy = null;
            return false;

        }
        //Set Copy into Dictionary
        private void SetCopiedObjectIntoDictionary(object instance, object instanceCopy)
        {
            if (!_copiedAndRealInstances.ContainsKey(instance))
                _copiedAndRealInstances[instance] = instanceCopy;
        }
        //DeepCopy
        private object DeepCopyHandler(object DeepObject, MemberInfo FieldOrPropertyMemberFromDeepObject)
        {

            SetValueInPropertyOrField(FieldOrPropertyMemberFromDeepObject, DeepObject, out var DeepinstanceOfPropertyOrFieldFromObject);

            if (DeepinstanceOfPropertyOrFieldFromObject is IList DeepObjectsEnumerable)
            {
                return CollectionDeepCopyHandler(DeepObjectsEnumerable);
            }

            if (DeepinstanceOfPropertyOrFieldFromObject != null)
            {
                return GetCopy(DeepinstanceOfPropertyOrFieldFromObject);
            }

            return default;
        }

        //ShallowCopy
        private object ShallowCopyHandler(object ShallowObject, MemberInfo ShallowObjectMemberInfo)
        {
            SetValueInPropertyOrField(ShallowObjectMemberInfo, ShallowObject, out var InstanceOfPropertyOrFieldFromShallowObject);

            return InstanceOfPropertyOrFieldFromShallowObject;
        }

        private object InstanceCreatingHandler(List<object> ArgumentsOfCtor, Type TypeOfCreatingObject)
        {
            //Exception
            ArgumentNullException(TypeOfCreatingObject);
            //Get All Constructors of Current Object 
            var Constructors = GetAllocatedCtorInProcessedObject(TypeOfCreatingObject);

            foreach (var Ctor in Constructors)
            {
                var CtorParameters = Ctor.GetParameters();
                //if the ArgumentsOfCtor And Constructors ParameterLength is Equals
                if (ArgumentsOfCtor.Count == CtorParameters.Length)
                {
                    bool allArgumentsMatch = true;

                    for (int i = 0; i < CtorParameters.Length; i++)
                    {
                        //if Argument of ctor wich Created And ParameterType of CtorParametr isAssigneble 
                        if (!CtorParameters[i].ParameterType.IsAssignableFrom(ArgumentsOfCtor[i].GetType()))
                        {
                            allArgumentsMatch = false;
                            break;
                        }
                    }

                    if (allArgumentsMatch)
                    {
                        //Creating Instance When AllArgumentsMatch


                        var InstanceOfObject = Activator.CreateInstance(TypeOfCreatingObject, ArgumentsOfCtor.ToArray());


                        return InstanceOfObject;
                    }
                }
            }
            //Exception
            throw new InvalidOperationException($"Failed to create an instance of type {TypeOfCreatingObject.Name}.");
        }

        






        //HelperMethods
        private CopyAttribute GetCopyAttribute(MemberInfo objectMemberInfo)
        {
            //
            var CopyAttribute = objectMemberInfo.
                GetCustomAttribute<CopyAttribute>();

            return CopyAttribute;
        }
        private CopyEnum GetCopyEnumFromAttribute(CopyAttribute? copyAttribute)
        {
            return copyAttribute == null ?
                CopyEnum.Shallow :
                copyAttribute.copy == CopyEnum.Deep ?
                CopyEnum.Deep : CopyEnum.Shallow;
        }
        private object SetValueInPropertyOrField(MemberInfo FieldOrPropertyMemberInfoFromObject, object Instance, out object instanceOfPropertyOrFieldFromObject)
        {

            //Exceptions
            ArgumentNullException(FieldOrPropertyMemberInfoFromObject);
            ArgumentNullException(Instance);



            instanceOfPropertyOrFieldFromObject = default(object);

            var TypeOfInstance = FieldOrPropertyMemberInfoFromObject.GetType();

            if (FieldOrPropertyMemberInfoFromObject is FieldInfo fieldinfo)
            {
                //SetFieldValueInInstance
                if (!IsSystematicFieldOrProperty(FieldOrPropertyMemberInfoFromObject, TypeOfInstance))
                {
                    try
                    {
                        instanceOfPropertyOrFieldFromObject = fieldinfo.GetValue(Instance);
                        fieldinfo.SetValue(Instance, instanceOfPropertyOrFieldFromObject);
                    }
                    catch
                    {
                        return instanceOfPropertyOrFieldFromObject;
                    }

                }

            }
            if (FieldOrPropertyMemberInfoFromObject is PropertyInfo propertyinfo)
            {
                //SetPropertyValueInInstance
                if (!IsSystematicFieldOrProperty(FieldOrPropertyMemberInfoFromObject, TypeOfInstance))
                {
                    try
                    {
                        if (FieldOrPropertyMemberInfoFromObject.Name != "Capacity")
                        {
                            instanceOfPropertyOrFieldFromObject = propertyinfo.GetValue(Instance);
                            propertyinfo.SetValue(Instance, instanceOfPropertyOrFieldFromObject);
                        }
                    }
                    catch
                    {
                        return instanceOfPropertyOrFieldFromObject;
                    }


                }
            }

            return instanceOfPropertyOrFieldFromObject;
        }

        private bool IsSystematicFieldOrProperty(MemberInfo FieldOrPropertyMemberInfoFromObject, Type InstanceType)
        {
            if (FieldOrPropertyMemberInfoFromObject is FieldInfo fieldInfo)
            {
                return fieldInfo.IsStatic || fieldInfo.Name.Contains("<");
            }

            if (FieldOrPropertyMemberInfoFromObject is PropertyInfo propertyInfo)
            {
                var accessorMethods = propertyInfo.GetAccessors();
                return accessorMethods.Any(method => method.IsStatic || method.Name.Contains("<"));
            }

            return false;
        }

        private MemberInfo[] GetMembersFromTypeOfProcessObject(Type processObjectType)
        {
            //Exception
            ArgumentNullException(processObjectType);

            var MembersInfo = processObjectType.GetMembers();

            return MembersInfo;
        }
        private ConstructorInfo[] GetAllocatedCtorInProcessedObject(Type processObjectType)
        {
            //Exception
            ArgumentNullException(processObjectType);

            return processObjectType.GetConstructors();
        }

        private void AddArgumentInstanceInObjectCtorCollection(MemberInfo CopyinstanceMemberInfo, object CopyInstance, List<object> CtorArguments)
        {
            //If Its a Systematic FiledAndProperty
            if (CopyinstanceMemberInfo == null)
                return;


            if (CopyinstanceMemberInfo is PropertyInfo || CopyinstanceMemberInfo is FieldInfo)
            {
                CtorArguments.Add(CopyInstance);
            }
        }


        //Exceptions
        private void ArgumentNullException<TArgument>(TArgument value)
        {
            if (value == null)
                throw new ArgumentNullException($"typeof {value?.GetType()}  Cannot be Null", nameof(value));
        }

    }

}
