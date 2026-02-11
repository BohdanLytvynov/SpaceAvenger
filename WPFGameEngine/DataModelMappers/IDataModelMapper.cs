namespace WPFGameEngine.DataModelMappers
{
    public interface IDataModelMapper<TDataModel>
        where TDataModel : class
    {
        void Map(TDataModel dataModel);
    }
}
