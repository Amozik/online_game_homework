using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFab
{
    public class PlayFabCatalog
    {
        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

        public PlayFabCatalog()
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
        }

        public Dictionary<string, CatalogItem> GetItems()
        {
            return _catalog;
        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            HandleCatalog(result.Catalog);
            Debug.Log($"Catalog was loaded successfully!");
        }
        
        private void HandleCatalog(List<CatalogItem> catalog)
        {
            foreach (var item in catalog)
            {
                _catalog.Add(item.ItemId, item);
                //Debug.Log($"Catalog item {item.ItemId} was added successfully!");
            }
        }
        
        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }
    }
}