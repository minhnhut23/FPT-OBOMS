using DataAccess.DAO;

namespace DataAccess.IRepository.Repository;

public class ManageShopRepository : IManageShopRepository
{
    private readonly ManageShopDAO manageShopDAO;

    public ManageShopRepository(ManageShopDAO manageShopDAO)
    {
        this.manageShopDAO = manageShopDAO;
    }
}
