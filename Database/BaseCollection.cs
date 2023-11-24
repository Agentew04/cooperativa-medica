using CoopMedica.Services;
using MySql.Data.MySqlClient;

namespace CoopMedica.Database;

/// <summary>
/// Classe abstrata de coleção que mapeia um tipo <typeparamref name="T"/> para uma tabela no banco de dados.
/// </summary>
/// <typeparam name="T">O tipo a ser mapeado na tabela</typeparam> 
public abstract class BaseCollection<T> where T : class {
    protected readonly MySqlConnection conn;

    /// <summary>
    /// Creates a new CollectionBase instance.
    /// </summary>
    protected BaseCollection() {
        conn = DatabaseService.Instance.Connection;
    }

    /**
     * Returns a list of all items in the collection.
     * @return The list of items
     */
    public async Task<IEnumerable<T>> SelectAsync() {
        List<T> list = new();
        
        MySqlCommand cmd = GetSelectSQL();
        cmd.Connection = conn;
        await using MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
        while(reader.Read()) {
            list.Add(ReadResult(reader));
        }
        return list;
    }

    /// <summary>
    /// Insere um item na colecao.
    /// </summary>
    /// <param name="item">O item a ser devolvido</param>
    /// <returns>Uma task representando a operacao assincrona</returns>
    public async Task AddAsync(T item) {
        MySqlCommand cmd = GetInsertSQL(item);
        cmd.Connection = conn;
        await cmd.PrepareAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Deleta um item da colecao. O item deve ter o campo Id preenchido.
    /// </summary>
    /// <param name="item">O item a ser deletado</param>
    public async Task RemoveAsync(T item) {
        MySqlCommand cmd = GetDeleteSQL(item);
        cmd.Connection = conn;
        await cmd.PrepareAsync();

        await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Deleta varios itens da colecao baseaado em um predicado
    /// </summary>
    /// <param name="predicate">O predicado que deletara os itens</param>
    /// <returns>Quantos itens vao ser deletados</returns>
    public async Task<int> RemoveAsync(Predicate<T> predicate){
        int deleted = 0;
        foreach(T item in await SelectAsync()){
            if(predicate.Invoke(item)){
                await RemoveAsync(item);
                deleted++;
            }
        }
        return deleted;
    }

    /// <summary>
    /// Atualiza uma entidade na colecao. O item deve ter o campo Id preenchido.
    /// 
    /// <param name="item">O item a ser atualizado</param>
    public async Task Update(T item){
        MySqlCommand cmd = GetUpdateSQL(item);
        cmd.Connection = conn;
        await cmd.PrepareAsync();
        
        await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Retorna o primeiro item que respeita um predicado
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public async Task<T?> SelectOneAsync(Predicate<T> predicate){
        foreach(T item in await SelectAsync()){
            if(predicate.Invoke(item)){
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// Retorna uma lista de itens que respeitam um predicado
    /// </summary>
    /// <param name="predicate">O predicado a ser testado</param>
    /// <returns>A lista de <typeparamref name="T"/> encontrados</returns>
    public async Task<List<T>> SelectAsync(Predicate<T> predicate){
        List<T> list = new();
        foreach(T item in await SelectAsync()){
            if(predicate.Invoke(item)){
                list.Add(item);
            }
        }
        return list;
    }

    /**
     * Returns true if any item matches the given predicate.
     * @param predicate The condition to test the items with
     * @return True if any item matches the predicate
     */
    public async Task<bool> Contains(Predicate<T> predicate) {
        foreach (T item in await SelectAsync()) {
            if(predicate.Invoke(item)){
                return true;
            }
        }
        return false;
    }

    /**
     * Returns a new instance of the item that is read from the ResultSet.
     * @param rs The ResultSet to read from
     * @return The new item
     */
    public abstract T ReadResult(MySqlDataReader rs);

    /**
     * Returns a PreparedStatement that selects all items from the database.
     * @param conn The connection that creates the statement
     * @return The Statement already filled with the item's data
     * @throws SQLException Thrown if an error occurs
     */
    protected abstract MySqlCommand GetSelectSQL();

    /**
     * Returns a PreparedStatement that inserts the given item into the database.
     * @param conn The connection that creates the statement
     * @param item The item to insert
     * @return The Statement already filled with the item's data
     */
    protected abstract MySqlCommand GetInsertSQL(T item);


    /**
     * Returns a PreparedStatement that deletes the given item from the database.
     * Should only compare items using the respective primary key/id.
     * @param conn The connection
     * @param item The item to delete
     * @return The Statement already filled with the item's data
     */
    protected abstract MySqlCommand GetDeleteSQL(T item);

    /**
     * Returns a PreparedStatement that updates the given item in the database.
     * Should only compare items using the respective primary key/id.
     * @param conn The connection that creates the statement
     * @param item The item that should be updated with its new data
     * @return The Statement already filled with the item's data
     */
    protected abstract MySqlCommand GetUpdateSQL(T item);
}
