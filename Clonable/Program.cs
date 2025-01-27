using DeepCopy.Attributes;
using DeepCopy.Enums;
using DeepCopy.Services;

namespace DeepCopySolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MyClass2 myClass21 = new MyClass2("Baghdasaryan1");
            MyClass2 myClass22 = new MyClass2("Baghdasaryan2");
            MyClass2 myClass23 = new MyClass2("Baghdasaryan3");
            List<MyClass2> values = new List<MyClass2>() { myClass21, myClass21, myClass21 };
            List<MyClass2> myClass2s = new List<MyClass2>(values);


            MyClass myClass = new MyClass("karo", myClass2s);
            CopyService<MyClass> copy = new CopyService<MyClass>(myClass);
            var CopymyClass = copy.GetCopy(myClass);

            //Not Reference Equals its a Deep Copy
            myClass.Name = "NewKaro";
            Console.WriteLine(((MyClass)CopymyClass).Name);

            Console.ReadLine();
        }
    }
    public class MyClass
    {
        [CopyAttribute(copy: CopyEnum.Shallow)]
        public string Name { get; set; }
        [CopyAttribute(copy: CopyEnum.Deep)]
        public List<MyClass2> MyClass2List { get; set; }
        public MyClass(string name, List<MyClass2> myClass2list)
        {
            Name = name;
            MyClass2List = myClass2list;
        }
    }

    public class MyClass2
    {
        public string Name2 { get; set; }
        public MyClass2(string name)
        {
            Name2 = name;
        }
    }

}