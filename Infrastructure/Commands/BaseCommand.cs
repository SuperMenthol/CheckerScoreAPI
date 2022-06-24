using Domain.Data.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Commands
{
    public abstract class BaseCommand
    {
        protected IDataContext _dataContext;

        public BaseCommand(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public abstract Task<ObjectResult> Execute();
    }
}