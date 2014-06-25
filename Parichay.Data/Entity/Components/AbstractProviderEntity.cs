namespace Parichay.Data.Entity
{
    /// <summary>
    /// Base Plain Old CLR Object (POCO) representing the common attributes of a provider objects.
    /// </summary>
    public abstract class AbstractProviderEntity
    {
        #region Fields
        private int id;
        private string name;
        private string loweredName;
        private string description;
        #endregion Fields

        #region Properties
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                LoweredName = value.ToLower();
            }
        }
        public string LoweredName
        {
            get { return loweredName; }
            set { loweredName = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion Properties

        #region Initialization
        public AbstractProviderEntity()
        {
        }
        public AbstractProviderEntity(string name)
        {
            Name = name;
        }
        #endregion Initialization
    }
}