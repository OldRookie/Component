using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Component.Infrastructure
{
    public static class MapExtension {
        public static TModel JsonAutoMapTo<TModel>(this object entity)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var json = JsonConvert.SerializeObject(entity);
            return JsonConvert.DeserializeObject<TModel>(json);
        }

        public static TModel AutoMapTo<TModel>(this object entity, TModel destination, MemberList memberList = MemberList.None)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(entity.GetType(), typeof(TModel), memberList);
                cfg.CreateMissingTypeMaps = true;
            });

            IMapper mapper = new Mapper(config);
            return mapper.Map(entity, destination);
        }

        public static TModel AutoMapTo<TModel>(this object entity, MemberList memberList = MemberList.None)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(entity.GetType(), typeof(TModel), memberList);
                cfg.CreateMissingTypeMaps = true;
            });

            IMapper mapper = new Mapper(config);
            return (TModel)mapper.Map(entity, entity.GetType(), typeof(TModel));
        }

        public static TModel MapToByConfig<TModel>(this object entity, TModel destination, MapperConfiguration config)
        {
            IMapper mapper = new Mapper(config);
            return mapper.Map(entity, destination);
        }

        public static Dest MapTo<Source, Dest>(this Source entity, Action<IMappingExpression<Source, Dest>> opt)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                opt(cfg.CreateMap<Source, Dest>(MemberList.None));
            });

            IMapper mapper = new Mapper(config);
            return (Dest)mapper.Map<Dest>(entity);
        }

        public static Dest MapTo<Dest>(this object entity,
            Action<IMapperConfigurationExpression> action, bool useCustomCfg = false)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                if (!useCustomCfg)
                {
                    if (entity.GetType().IsGenericType)
                    {
                        cfg.CreateMap(entity.GetType().GenericTypeArguments[0]
                            , typeof(Dest).GenericTypeArguments[0], MemberList.None);
                    }
                    else
                    {
                        cfg.CreateMap(entity.GetType(), typeof(Dest), MemberList.None);
                    }
                }

                action(cfg);
            });

            IMapper mapper = new Mapper(config);
            return (Dest)mapper.Map<Dest>(entity);
        }

        public static Dest MapTo<Dest>(this object entity, Dest dest,
           Action<IMapperConfigurationExpression> action, bool useCustomCfg = false)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                if (!useCustomCfg)
                {
                    if (entity.GetType().IsGenericType)
                    {
                        cfg.CreateMap(entity.GetType().GenericTypeArguments[0]
                            , typeof(Dest).GenericTypeArguments[0], MemberList.None);
                    }
                    else
                    {
                        cfg.CreateMap(entity.GetType(), typeof(Dest), MemberList.None);
                    }
                }

                action(cfg);
            });

            IMapper mapper = new Mapper(config);
            return (Dest)mapper.Map(entity, dest, entity.GetType(), typeof(Dest));
        }

        public static Dest MapTo<Source, Dest>(this Source entity, Dest destination, Action<IMappingExpression<Source, Dest>> opt)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                opt(cfg.CreateMap<Source, Dest>(MemberList.None));
            });

            IMapper mapper = new Mapper(config);
            return (Dest)mapper.Map(entity, destination);
        }
    }
}