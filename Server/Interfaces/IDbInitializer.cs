using System.Threading.Tasks;

namespace RecrutariBlazorWasm.Server.Interfaces
{
    public interface IDbInitializer
    {
        Task InitializeData();
    }
}
