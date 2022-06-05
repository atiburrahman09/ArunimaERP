using ScopoERP.Domain.Repositories;
using ScopoERP.UserManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.UserManagement.BLL
{
    public class UserLogic
    {
        private UnitOfWork unitOfWork;

        public UserLogic(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Guid GetUserID(string userName)
        {
            Guid userID = (from s in unitOfWork.UserRepository.Get()
                          where s.UserName == userName
                          select s.UserId).SingleOrDefault();

            return userID;
        }

        public int GetAccountID(Guid userID)
        {
            var result = (from s in unitOfWork.UserAccountRepository.Get()
                          where s.Status == 1
                            where s.UserId == userID
                              select s).SingleOrDefault();

            if (result == null)
                return 0;

            return result.AccountId;
        }

        public IEnumerable<UserViewModel> GetAll()
        {

            var data = (from u in unitOfWork.UserRepository.Get()
                        join uc in unitOfWork.UserAccountRepository.Get()
                        on u.UserId equals uc.UserId into uc_group
                        from acc in uc_group.DefaultIfEmpty()
                        select new UserViewModel
                        {
                            AccountId = acc.AccountId,
                            AccountName = acc.account.AccountName,
                            Email = u.Memberships.Email,
                            UserName = u.UserName,
                            RoleNames = u.Roles
                                         .Select(x => x.RoleName)
                                         .ToList()
                        }).AsEnumerable();            
            return data;
        }

        public UserViewModel GetByUserName(string userName)
        {

            var data = (from u in unitOfWork.UserRepository.Get()
                        join uc in unitOfWork.UserAccountRepository.Get()
                        on u.UserId equals uc.UserId into uc_group
                        from acc in uc_group.DefaultIfEmpty()
                        where u.UserName == userName
                        select new UserViewModel
                        {
                            AccountId = acc.AccountId,
                            AccountName = acc.account.AccountName,
                            Email = u.Memberships.Email,
                            UserName = u.UserName,
                            RoleNames = u.Roles
                                         .Select(x => x.RoleName)
                                         .ToList(),
                            UserID = u.UserId
                        }).SingleOrDefault();
            return data;

            //return unitOfWork.UserRepository.Get()
            //    .Where(x => x.UserName == userName)
            //    .Select(x => new UserViewModel
            //    {
            //        UserName = x.UserName,
            //        RoleNames = x.Roles
            //                    .Select(y => y.RoleName)
            //                    .ToList(),
            //        Email = x.Memberships.Email
                    
            //    })
            //    .SingleOrDefault();
        }
    }
}
