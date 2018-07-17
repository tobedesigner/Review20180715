using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class ClientRepository : EFRepository<Client>, IClientRepository
	{
        public override IQueryable<Client> All()
        {
            return base.All().Where(c => c.IsDelete == false);
        }

        public override void Delete(Client entity)
        {
            entity.IsDelete = true;
        }

        public Client Find(int id)
        {
            return All().FirstOrDefault(c => c.ClientId == id);
        } 
    }

	public  interface IClientRepository : IRepository<Client>
	{

	}
}