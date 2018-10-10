using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using AbpCompanyName.AbpProjectName.EntityFrameworkCore;

namespace AbpCompanyName.AbpProjectName.Tests.Proposals
{
    public class ProposalRepository_Tests : AbpProjectNameTestBase
    {
        private readonly IRepository<Proposal> _proposalRepository;
        private readonly IRepository<TravelOfferPriceProposalElement> _proposalElementRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProposalRepository_Tests()
        {
            _proposalRepository = Resolve<IRepository<Proposal>>();
            _proposalElementRepository = Resolve<IRepository<TravelOfferPriceProposalElement>>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task FilterProposals_Test()
        {
            await _proposalRepository.InsertAsync(new Proposal
            {
                TravelOfferPriceProposalElements = new List<TravelOfferPriceProposalElement>
                {
                    new TravelOfferPriceProposalElement()
                }
            });

            using (var uow = _unitOfWorkManager.Begin())
            {
                var proposal = await _proposalRepository.GetAll()
                    .Include(b => b.TravelOfferPriceProposalElements)
                    .FirstAsync(b => b.Id == 0);

                proposal.TravelOfferPriceProposalElements = new List<TravelOfferPriceProposalElement>();

                await uow.CompleteAsync();
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                _proposalElementRepository.Count().ShouldBe(0);

                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    _proposalElementRepository.Count().ShouldBe(1);
                }

                await uow.CompleteAsync();
            }
        }
    }
}
