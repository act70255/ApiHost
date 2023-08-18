using Autofac;
using AutoMapper;
using DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiHost.DNDHost.Setup.Module
{
    public class AutoMapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                //Register Mapper Profile
                cfg.AddProfile<AutoMapperProfile>();
            }
            )).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
        }
    }

    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Creature, Controller.CreatureRequest>()
                //.ForMember(request => request.Name, opt => opt.MapFrom(Model => Model.Name))
                //.ForMember(request => request.HealthValue, opt => opt.MapFrom(Model => Model.Health.Value))
                //.ForMember(request => request.ManaValue, opt => opt.MapFrom(Model => Model.Mana.Value))
                //.ForMember(request => request.StaminaValue, opt => opt.MapFrom(Model => Model.Stamina.Value))
                //.ForMember(request => request.ExperienceValue, opt => opt.MapFrom(Model => Model.Experience.Value))
                //.ForMember(request => request.LevelValue, opt => opt.MapFrom(Model => Model.Level.Value))
                //.ForMember(request => request.ArmorClassValue, opt => opt.MapFrom(Model => Model.ArmorClass.Value))
                //.ForMember(request => request.AttackBonusValue, opt => opt.MapFrom(Model => Model.AttackBonus.Value))
                //.ForMember(request => request.DamageValue, opt => opt.MapFrom(Model => Model.Damage.Value))
                //.ForMember(request => request.StrengthValue, opt => opt.MapFrom(Model => Model.Strength.Value))
                //.ForMember(request => request.DexterityValue, opt => opt.MapFrom(Model => Model.Dexterity.Value))
                //.ForMember(request => request.IntelligenceValue, opt => opt.MapFrom(Model => Model.Intelligence.Value))
                //.ForMember(request => request.CharismaValue, opt => opt.MapFrom(Model => Model.Charisma.Value))
                //.ForMember(request => request.Skills, opt => opt.MapFrom(Model => Model.Skills))
                .ReverseMap();
        }
    }
}
