using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace MonoGame.EcGuiLab;

public static class Tests
{
    public static void Run()
    {
        Activator.CreateInstance<ClassA>();
        Activator.CreateInstance<ClassB>();
        Activator.CreateInstance(typeof(ClassC), true);
        try {
            Activator.CreateInstance<ClassC>();
        } catch (MissingMethodException e) {
            if (!e.Message.StartsWith("No parameterless constructor defined for type")) throw new InvalidOperationException();
        }
        
        CreateInstance<ClassA>();
        CreateInstance<ClassB>();
        CreateInstance<ClassC>();
    }
    
    private static T CreateInstance<T>() {
        var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
        return (T)constructor!.Invoke(null);
    }
    
    class ClassA { }
    
    class ClassB {
        public ClassB() {}
    }
    
    class ClassC {
        private ClassC() {}
    }

}