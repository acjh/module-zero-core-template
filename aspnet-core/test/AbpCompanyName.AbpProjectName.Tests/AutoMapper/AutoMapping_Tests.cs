using Abp.Application.Services.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AbpCompanyName.AbpProjectName.Tests.AutoMapper
{
    public class AutoMapping_Tests : AbpProjectNameTestBase
    {
        private readonly IMapper _mapper;

        public AutoMapping_Tests()
        {
            var config = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<Plan, PlanDto>()
                    .ForMember(dto => dto.ChildPlan, options => options.MapFrom(
                        p => new object[] { p.PlanChild1, p.PlanChild2 }.Where(c => c != null)));
                
                configuration.CreateMap<PlanDetail, ChildPlan>()
                   .ForMember(dto => dto.Type, options => options.MapFrom(p => ChildPlanType.PlanDetail));

                configuration.CreateMap<ObservationCare, ChildPlan>()
                    .ForMember(dto => dto.Type, options => options.MapFrom(p => ChildPlanType.ObservationCare));
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Map_Test()
        {
            var plans = new List<Plan> {
                new Plan
                {
                    Date = DateTime.Now,
                    Id = 1,
                    PlanChild1 = new PlanDetail
                    {
                        Description = nameof(PlanDetail),
                        Id = 1
                    },
                    PlanChild2 = null
                }
            };

            var output = _mapper.Map<List<PlanDto>>(plans);
        }

        #region Entities

        public class Plan
        {
            public virtual int Id { get; set; }
            public DateTime Date { get; set; }
            public virtual PlanDetail PlanChild1 { get; set; }
            public virtual ObservationCare PlanChild2 { get; set; }
        }

        public class PlanDetail
        {
            public virtual int Id { get; set; }
            public virtual Plan Plan { get; set; }
            public virtual string Description { get; set; }
        }

        public class ObservationCare
        {
            public virtual int Id { get; set; }
            public virtual Plan Plan { get; set; }
            public virtual string Description { get; set; }
        }

        #endregion

        #region DTOs

        public class PlanDto : EntityDto
        {
            public DateTime Date { get; set; }
            public IEnumerable<ChildPlan> ChildPlan { get; set; }
        }

        public class ChildPlan : EntityDto
        {
            public ChildPlanType Type { get; set; }
        }

        public enum ChildPlanType
        {
            PlanDetail,
            ObservationCare
        }

        #endregion
    }
}
