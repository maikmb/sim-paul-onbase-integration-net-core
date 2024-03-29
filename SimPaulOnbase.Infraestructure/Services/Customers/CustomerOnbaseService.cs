﻿using Hyland.Unity;
using Hyland.Unity.UnityForm;
using SimPaulOnbase.Application.Services;
using SimPaulOnbase.Core.Customers;

namespace SimPaulOnbase.Infraestructure.Services.Customers
{
    /// <summary>
    /// CustomerOnbaseIntegration class
    /// </summary>
    public class CustomerOnbaseService : OnbaseServiceBase, ICustomerOnbaseService
    {
        private Customer _customer;
        private readonly OnbaseSettings _onbaseSettings;
        public CustomerOnbaseService(OnbaseSettings onbaseSettings) : base(onbaseSettings)
        {
            this._onbaseSettings = onbaseSettings;
        }

        public void Handle(Customer customer)
        {
            this._customer = customer;
            IntegrateCustomer();
        }

        private void IntegrateCustomer()
        {
            FormTemplate formTemplate = this.FindFormTemplate(_onbaseSettings.FormIntegrationID);
            StoreNewUnityFormProperties onbaseStore = this.InitNewForm(formTemplate);
            MapCustomerFieldsToOnbase(_customer, onbaseStore);

            var onbaseDocument = this.StoreNewUnityForm(onbaseStore);
            if (onbaseDocument != null)
            {
                this.UploadUnityFormImage(_customer.Photo, "CPF", _customer.Document);
            }
        }

        private void MapCustomerFieldsToOnbase(Customer customer, StoreNewUnityFormProperties onbaseStore)
        {
            onbaseStore.AddKeyword("Código do Cliente", customer.Id);
            onbaseStore.AddKeyword("Nome do Cliente", customer.Name);
            onbaseStore.AddKeyword("CPF", customer.Document);
            onbaseStore.AddKeyword("Nome da Mãe", customer.Mother);
            onbaseStore.AddKeyword("E-Mail", customer.Email);

            //onbaseStore.AddField("COD_CLIENTE", customer.Id);
            //onbaseStore.AddField("NOME_CLIENTE", customer.Name);
            //onbaseStore.AddField("NOME_MAE_CLIENTE", customer.Mother);
            //onbaseStore.AddField("EMAIL", customer.Email);
        }
    }
}
