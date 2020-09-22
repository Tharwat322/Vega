using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Vega.Controllers.Resources;
using Vega.Core.Models;

namespace Vega.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {

            // Domain to Resource
            CreateMap<Photo, PhotoResource>();
            CreateMap<Make, MakeResource>(); 
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Make, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<VehicleQueryResource,VehicleQuery>();
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));

            CreateMap<Vehicle, SaveVehicleResource>()
            .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Phone = v.ContactPhone, Email = v.Email }))
            .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));

            CreateMap<Vehicle, VehicleResource>()
            .ForMember(vr => vr.Make , opt => opt.MapFrom( v => v.Model.Make))
              .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Phone = v.ContactPhone, Email = v.Email }))
              .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => new KeyValuePairResource { Id = vf.Feature.Id, Name = vf.Feature.Name} )));
            //Resource to Domain

            CreateMap<SaveVehicleResource, Vehicle>()
            .ForMember(v => v.Id, opt => opt.Ignore())
            .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
            .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
            .ForMember(v => v.Email, opt => opt.MapFrom(vr => vr.Contact.Email))
            .ForMember(v => v.Features, opt => opt.Ignore())
            .AfterMap((vr, v) =>
            {

                // removed un selected features based on user
                var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId)).ToList();
                foreach (var f in removedFeatures)
                    v.Features.Remove(f);

                // add new feature  


                var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id => new VehicleFeature { FeatureId = id }).ToList();
                foreach (var f in addedFeatures)
                    v.Features.Add(f);

            });


        }
    }
}