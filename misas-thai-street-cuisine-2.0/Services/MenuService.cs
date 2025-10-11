using misas_thai_street_cuisine_2._0.Models;
using misas_thai_street_cuisine_2._0.Data;

namespace misas_thai_street_cuisine_2._0.Services
{
    public class MenuService
    {
        public ICartItem? GetMenuItem(string name, string category)
        {
            if (category == "Platter")
            {
                return MenuData.Platters.FirstOrDefault(p => p.Name == name);
            }
            else if (category == "Side")
            {
                return MenuData.SideDishes.FirstOrDefault(s => s.Name == name);
            }
            return null;
        }
    }
}