﻿using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using MyExpenses.Views;
using MyExpenses.ViewModels;
using MyExpenses.Models;

namespace MyExpenses.Pages
{
	public class ReportsPage : BasePage
	{
		public ReportsPageViewModel ViewModel { get; set; }

		ReportListView list;
		ToolbarItem addReport, filterIcon;

		public ReportsPage()
		{
			NavigationPage.SetTitleIcon(this, "icon.png");
			ViewModel = new ReportsPageViewModel();
			BindingContext = ViewModel;
			AutomationId = "reportsPage";
			Title = "My Expense Reports";
			Content = list;

			#region Set Event Handlers and Bindings
			addReport.Clicked += HandleNewReport;
			filterIcon.Clicked += HandleFilterReports;
			list.ItemSelected += HandleReportSelected;

			list.SetBinding(ListView.ItemsSourceProperty, "Reports");
			#endregion
		}

		#region Create UI

		public override void ConstructUI()
		{
			base.ConstructUI();

			list = new ReportListView
			{
				AutomationId = "reportListView",
				IsPullToRefreshEnabled = true,
				RefreshCommand = new Command(async () =>
				{
					await ViewModel.RefreshData();
					list.IsRefreshing = false;
				})
			};
			addReport = new ToolbarItem { AutomationId = "addReport", Icon = "Plus_Add.png" };
			filterIcon = new ToolbarItem { AutomationId = "filterReports", Icon = "filter.png" };
		}

		public override void AddChildrenToParentLayout()
		{
			base.AddChildrenToParentLayout();

			ToolbarItems.Add(addReport);
			ToolbarItems.Add(filterIcon);
		}

		#endregion

		#region Event Handlers

		async void HandleReportSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var report = e.SelectedItem as ExpenseReport;
			await Navigation.PushAsync(new ReportDetailPage(report));
		}

		async void HandleFilterReports(object sender, EventArgs e)
		{
			var result = await DisplayActionSheet("Pick Filter", "Cancel", "All", "Pending Approval", "Pending Submission", "Approved");
			ViewModel.FilterData(result);
		}

		void HandleNewReport(object sender, EventArgs e)
		{
			Navigation.PushAsync(new NewReportPage());
		}

		#endregion

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Delay(3000);
			await ViewModel.RefreshData();
		}
	}
}