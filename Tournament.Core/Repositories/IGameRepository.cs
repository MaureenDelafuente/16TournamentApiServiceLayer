﻿using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync(int pageSize, int page);
    Task<Game?> GetAsync(int id, bool trackChanges = false);
    Task<Game?> GetAsync(string title);

    Task<bool> AnyAsync(int id);
    Task Add(Game game);
    void Update(Game game);
    void Remove(Game game);
    Task<int> CountAsync();
}