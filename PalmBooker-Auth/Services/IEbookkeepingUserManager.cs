using EBookkeepingAuth.Data;
using EBookkeepingAuth.Data.CompanyContext;
using EBookkeepingAuth.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBookkeepingAuth.Services
{
    public class EbookkeepingUserManager : IEbookkeepingUserManager
    {
        private readonly ApplicationDbContext _context;
        private readonly EbookkeepingApplicationContext _companyContext;


        public EbookkeepingUserManager(ApplicationDbContext context, EbookkeepingApplicationContext companyContext)
        {
            _context = context;
            _companyContext = companyContext;
        }

        public async Task AddCompanyAssociationAsync(CompanyAssociation companyAssociation)
        {
            try
            {
                await _companyContext.CompanyAssociation.AddAsync(companyAssociation);
                if (await _companyContext.SaveChangesAsync() < 1)
                {
                    throw new Exception("Error saving");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Companies> AddCompanyAsync(Companies company)
        {
            try
            {
                var response = await _companyContext.Companies.AddAsync(company);
                if (await _companyContext.SaveChangesAsync() == 1)
                {
                    return response.Entity;
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Commit()
        {
            _companyContext.SaveChanges();
        }

        public async Task<ApplicationUser> FindByPhoneasync(string phoneNumber)
        {
            return await _context.Users.Where(c => c.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task<bool> Save()
        {
            return (await _companyContext.SaveChangesAsync() >= 0);
        }
    }
    public interface IEbookkeepingUserManager
    {
        Task<ApplicationUser> FindByPhoneasync(string phoneNumber);
        Task<bool> Save();
        void Commit();
        Task<Companies> AddCompanyAsync(Companies company);
        Task AddCompanyAssociationAsync(CompanyAssociation companyAssociation);


    }
}
