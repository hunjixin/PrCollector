namespace PrCollector;
using System;
using System.Text;
using Octokit;
using Octokit.Internal;
using System.ComponentModel;

using System.Collections.ObjectModel;
using System.Windows.Input;

public partial class MainPage : ContentPage
{
	private readonly IFolderPicker _folderPicker;


   	static InMemoryCredentialStore credentials = new InMemoryCredentialStore(new Credentials("xxx"));
    static GitHubClient client = new GitHubClient(new ProductHeaderValue("user"), credentials);
	PrListViewModel model =  new PrListViewModel();
	public MainPage()
	{
		
#if WINDOWS
		_folderPicker = new Platforms.Windows.FolderPicker();
#elif MACCATALYST
		_folderPicker = new Platforms.MacCatalyst.FolderPicker();
#endif

		this.BindingContext = model;
		InitializeComponent();
	}

	private async void OnPrQueryClicked(object sender, EventArgs e)
	{
		var opts = new ApiOptions();
		opts.PageCount = 1 ;
		opts.PageSize = 100;
		opts.StartPage = 1;

		var prRequest = new PullRequestRequest();
		prRequest.State =ItemStateFilter.Closed;

		this.model.PRList.Clear();
		foreach (var projectCfg in this.model.Projects)
		{
			var requests = await client.PullRequest.GetAllForRepository(projectCfg.ProjectName, projectCfg.RepoName, prRequest, opts);
			foreach(var x in requests ){
				
				var diff = model.PrFromTime - x.CreatedAt;
				if (x.Merged && diff.Days>0 && diff.Days <8) {
					this.model.PRList.Add(new PrModel(projectCfg.RepoName, x.Title, x.HtmlUrl, x.Id, x.Number, x.State.ToString(), x.Base.Ref, x.CreatedAt));
				}
			}
		}
	}

	private async void OnOpenPr(object sender, EventArgs e)
	{
		try
		{
		var showModel = (sender as ImageButton).BindingContext as PrModel;
			Uri uri = new Uri(showModel.Url);
			await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
		}
		catch (Exception ex)
		{
			await DisplayAlert("error", ex.Message, "Ok");
		}
	}


	private void OnDeleteRow(object sender, EventArgs e)
	{
		var delModel = (sender as ImageButton).BindingContext as PrModel;
		model.PRList.Remove(delModel);
	}

	private  async void OnExport(object sender, EventArgs e) {
		try {
			var count = 1;
			var sb = new StringBuilder();
			foreach (var pr in model.PRList) {
				sb.AppendLine($"{count}. {pr.Titile}({pr.Component}#{pr.Number})");
				sb.AppendLine($"	{pr.Url}");
				count ++;
			}
			var pickedFolder = await _folderPicker.PickFolder();
			pickedFolder = pickedFolder.Replace("file://", "");
			var path = $"{pickedFolder}/prlist_{model.PrFromTime.ToString("yyyy_mm_dd")}.txt";
			File.WriteAllText(path, sb.ToString());
			await DisplayAlert("alert", $"write pr to {path} successful", "OK");
		}
		catch (Exception ex)
		{
			await DisplayAlert("error", ex.Message, "Ok");
		}
	}
}


public class PrModel {
	public string Component {get;set;}
	public string Titile{get;set;}
	public string Url {get;set;}
	public long Id {get;set;}
	public int Number {get;set;}
	public string State {get;set;}
	public string BaseRef {get;set;}
	public DateTimeOffset CreateAt {get;set;}
	public PrModel(string component, string title, string url, long id, int number, string state, string baseRef, DateTimeOffset createAt) {
		Titile = title;
		Url = url;
		Id=id;
		Number =number;
		State = state;
		BaseRef = baseRef;
		CreateAt = createAt;
		Component = component;
	}
}

public struct ProjectConfig {
	public string ProjectName {get;set;}
	public string RepoName {get;set;}

	public ProjectConfig(string projectName , string repoName) {
		ProjectName = projectName;
		RepoName = repoName;
	}
}
