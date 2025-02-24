public interface ISaveLoadService
{
    public bool HasChanges(ISavable objectToCheck);
    public void Save(ISavable objectToSave);
    public void Load(ISavable objectToOverwrite);
}