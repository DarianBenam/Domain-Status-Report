# Domain Status Report

![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/DarianBenam/Domain-Status-Report)
![GitHub Actions deploy workflow status](https://img.shields.io/github/actions/workflow/status/DarianBenam/Domain-Status-Report/.github%2Fworkflows%2Fdeploy.yaml?label=deploy%20workflow)
![Live demo website](https://img.shields.io/website?down_color=red&down_message=offline&label=live%20demo%20website&up_message=online&url=https%3A%2F%2Fstatus.darianbenam.com%2F)

A `.NET 6` [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) app that allows users to view the status of all domains owned and operated by Darian Benam. 🚀 This app was built using Visual Studio Community 2022.

## How to Run

After cloning the repository, navigate to the directory which holds the project files and run the following commands:

```console
npm install
dotnet run
```

You can also run the project via an IDE (such as Visual Studio or Visual Studio Code).

## Live Demo

To view a live demo of this app, visit https://status.darianbenam.com/. The app is deployed on an [NGINX](https://www.nginx.com/) instance running on Ubuntu 22.04 LTS (Jammy Jellyfish).

## Screenshots

![Homepage showing non-cached results](Screenshots/homepage.png "Homepage showing non-cached results")
![Homepage showing cached results](Screenshots/homepage-cache.png "Homepage showing cached results")
