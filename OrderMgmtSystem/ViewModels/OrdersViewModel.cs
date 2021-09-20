﻿using DataModels;
using DataProvider;
using System.Collections.ObjectModel;
using System.Linq;

namespace OrderMgmtSystem.ViewModels
{
    /// <summary>
    /// This class is responsible of the presentation logic of the UI(OrdersView). Provides methods and properties
    /// that can be bound to the UI.
    /// </summary>
    public class OrdersViewModel : ViewModelBase
    {
        private readonly IOrdersDataProvider _OrdersData;
        private Order _SelectedOrder;

        public OrdersViewModel(IOrdersDataProvider ordersData)
        {
            _OrdersData = ordersData;
            Orders = new ObservableCollection<Order>(_OrdersData.Orders);
        }
        public ObservableCollection<Order> Orders { get; }
        public Order SelectedOrder
        {
            get => _SelectedOrder;
            set => _SelectedOrder = value;
        }

        public void DeleteOrder(int id)
        {
            Orders.Remove(Orders
                .FirstOrDefault(order => order.Id.Equals(id)));
            // TODO: Add logic to update DB
        }
    }
}
