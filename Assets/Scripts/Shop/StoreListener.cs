using UnityEngine;
using UnityEngine.Purchasing;

public class StoreListener : MonoBehaviour, IStoreListener
{
    public IStoreController StoreController { get; private set; }

    public static StoreListener Instance;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, ProductCatalog.LoadDefaultCatalog());
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.LogWarning("Store initialization complete");
        StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("Store initialize failed " + error.ToString());
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purhchase failed " + product.definition.id + " Reason: " + failureReason.ToString());
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.LogWarning("Purhchase success " + purchaseEvent.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }
}