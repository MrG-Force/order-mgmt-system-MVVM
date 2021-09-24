﻿using DataModels;
using OrderMgmtSystem.Commands;
using OrderMgmtSystem.CommonEventArgs;
using OrderMgmtSystem.ViewModels;
using OrderMgmtSystem.ViewModels.BaseViewModels;
using System.Collections.ObjectModel;

namespace OrderMgmtSystem.Services.Windows
{
    public class ChildWindowViewModel : ViewModelBase, IHandleOneOrder
    {
        private ViewModelBase _currentViewModel;
        private OrderDetailsViewModel _orderDetailsVM;
        private EditOrderViewModel _editOrderVM;
        private AddItemViewModel _addItemVM;
        private bool _isModalOpen;

        public ChildWindowViewModel(OrderDetailsViewModel orderDetailsVM, EditOrderViewModel editOrderVM, AddItemViewModel addItemVM)
        {
            _orderDetailsVM = orderDetailsVM;
            _orderDetailsVM.EditOrderRequested += OrderDetailsVM_EditOrderRequested;
            _editOrderVM = editOrderVM;
            _editOrderVM.OrderUpdated += EditOrderVM_OrderUpdated;
            _editOrderVM.OrderItemRemoved += EditOrderVM_OrderItemRemoved;
            _addItemVM = addItemVM;
            _addItemVM.EditingOrderItemSelected += AddItemVM_EditingOrderItemSelected;
            _currentViewModel = orderDetailsVM;
            _isModalOpen = false;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void EditOrderVM_OrderItemRemoved(object sender, OrderItemRemovedEventArgs e)
        {
            _addItemVM.ReturnItemToStockList(e.StockItemId, e.Quantity, e.OnBackOrder);
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }
        public OrderDetailsViewModel OrderDetailsVM => _orderDetailsVM;
        private EditOrderViewModel EditOrderVM => _editOrderVM;
        public AddItemViewModel AddItemVM => _addItemVM;
        public bool IsModalOpen
        {
            get => _isModalOpen;
            set => SetProperty(ref _isModalOpen, value);
        }
        public Order Order { get => OrderDetailsVM.Order; set => Order = value; }

        public DelegateCommand<string> NavigateCommand { get; private set; }
        private void EditOrderVM_OrderUpdated()
        {
            EditOrderVM.TempOrder = new Order(OrderDetailsVM.Order);
            EditOrderVM.TempOrderItems = new ObservableCollection<OrderItem>(OrderDetailsVM.Order.OrderItems);
            Navigate("OrderDetailsView");
        }

        private void OrderDetailsVM_EditOrderRequested()
        {
            Navigate("EditOrderView");
        }

        private void AddItemVM_EditingOrderItemSelected(OrderItem item)
        {
            EditOrderVM.AddOrderItem(item);
            Navigate("CloseAddItemView");
        }

        private void Navigate(string destination)
        {
            switch (destination)
            {
                case "EditOrderView":
                    CurrentViewModel = EditOrderVM;
                    break;
                case "AddItemView":
                    IsModalOpen = true;
                    break;
                case "CloseAddItemView":
                    IsModalOpen = false;
                    break;
                case "OrderDetailsView":
                default:
                    CurrentViewModel = OrderDetailsVM;
                    break;
            }
        }
    }
}
