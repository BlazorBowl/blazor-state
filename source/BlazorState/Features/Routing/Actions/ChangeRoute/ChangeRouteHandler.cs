﻿namespace BlazorState.Features.Routing
{
  using System.Threading;
  using System.Threading.Tasks;
  using BlazorState;
  using Microsoft.AspNetCore.Components;

  public partial class RouteState
  {
    internal class ChangeRouteHandler : RequestHandler<ChangeRouteAction, RouteState>
    {
      public ChangeRouteHandler(
        IStore aStore,
        IUriHelper aUriHelper
        ) : base(aStore)
      {
        UriHelper = aUriHelper;
      }

      private RouteState RouteState => Store.GetState<RouteState>();

      private IUriHelper UriHelper { get; }

      public override Task<RouteState> Handle(ChangeRouteAction aChangeRouteRequest, CancellationToken aCancellationToken)
      {
        string newAbsoluteUri = UriHelper.ToAbsoluteUri(aChangeRouteRequest.NewRoute).ToString();
        string absoluteUri = UriHelper.GetAbsoluteUri();

        if (absoluteUri != newAbsoluteUri)
        {
          // RouteManager OnLocationChanged will fire this ChangeRouteRequest again 
          // and the second time we will hit the else clause.
          UriHelper.NavigateTo(newAbsoluteUri);
        }
        else if (RouteState.Route != newAbsoluteUri)
        {
          RouteState.Route = newAbsoluteUri;
        }

        return Task.FromResult(RouteState);
      }
    }
  }
}