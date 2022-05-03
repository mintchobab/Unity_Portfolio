[System.Serializable]
public class Item
{
    public ItemData ItemData { get; private set; }

    public int CurrentCount { get; private set; } = 1;


    public void SetItemData(ItemData itemData)
    {
        this.ItemData = itemData;
    }

    public void IncreaseItemCount(int count)
    {
        CurrentCount += count;
    }

    public void DecreseItemCount(int count)
    {
        CurrentCount -= count;
    }
}
