using System.Collections.Generic;
using Cinema.Entities;

namespace Cinema.Interfaces
{
    public interface IFavoriteService
    {
        List<int> GetIds();
        List<Movie> GetAll();
        void Add(int id);
        void Remove(int id);
        void Clear();
        int GetCount();
    }
}
