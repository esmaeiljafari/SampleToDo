﻿using System.Reflection;
using AutoMapper;

namespace SampleToDo.Application.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        ApplyMappingProfiles(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingProfiles(Assembly assembly)
    {
        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICreateMapper<>)))
            .ToList();


        foreach (var type in types)
        {
            var model = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Map") //get the map method directly by the class
                             ?? type.GetInterface("ICreateMapper`1")
                                 .GetMethod("Map"); //if null get the interface implementation

            if (model != null)
                methodInfo?.Invoke(model, new object[]
                {
                    this
                });
        }
    }
}