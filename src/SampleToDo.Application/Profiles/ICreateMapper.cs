using AutoMapper;

namespace SampleToDo.Application.Profiles;

public interface ICreateMapper<TSource>
{
    public void Map(Profile profile)
    {
        profile.CreateMap(typeof(TSource), GetType()).PreserveReferences().ReverseMap();
    }
}