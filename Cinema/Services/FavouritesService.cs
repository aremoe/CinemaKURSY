using Microsoft.AspNetCore.Http;
using Cinema.Data;
using Cinema.Extensions;
using Cinema.Models;
using Cinema.Entities;



namespace Cinema;

public class FavouritesService
{
    private readonly ISession _session;
    private readonly CinemaDbContext _context;
    private const string key = "favourite_list";

    public FavouritesService(IHttpContextAccessor accessor, CinemaDbContext context)
    {
        this._session = accessor.HttpContext.Session;
        _context = context;
    }

    public List<int> GetIds()
    {
        return _session.Get<List<int>>(key) ?? new();
    }
    public List<Film> GetAll()
    {
        var ids = GetIds();
        return _context.FilmTeams4.Where(x => ids.Contains(x.Id)).ToList();
    }

    public void Add(int id)
    {
        var ids = GetIds();

        if (ids.Contains(id)) return;

        ids.Add(id);
        _session.Set(key, ids);
    }

    public void Remove(int id)
    {
        var ids = GetIds();
        ids.Remove(id);
        _session.Set(key, ids);
    }

    public void Clear()
    {
        _session.Remove(key);
    }
    public int GetCount()
    {
        return GetIds().Count;
    }
}