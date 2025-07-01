using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models.DTOs;
using news_application.Context;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class ExternalSourceRepository : GenericRepository<ExternalSource>, IExternalSourceRepository
    {
        private readonly NewsDataContext _context;

        public ExternalSourceRepository(NewsDataContext context) : base(context)
        {
            _context = context;
        }
       
    }
}
