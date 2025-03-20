using System;

namespace Allgem01.Logic;

using Microsoft.AspNetCore.Components;

public class Navigation
{
    private readonly NavigationManager NavigationManager;

    public Navigation(NavigationManager navigationManager)
    {
        NavigationManager = navigationManager;
    }
    public void Home()
    {

        //if ()
        //{
            //NavigationManager.NavigateTo("/home");    
        //}
        //else
        //{
            //NavigationManager.NavigateTo("/");
        //}

    }

    public void AboutUs()
    {
        NavigationManager.NavigateTo("/about-us");
    }

    public void ContactUs()
    {
        NavigationManager.NavigateTo("/contact-us");
    }


    public void Game1()
    {
        NavigationManager.NavigateTo("/game1");
    }

    public void Game2()
    {
        NavigationManager.NavigateTo("/game2");
    }
    public void Game3()
    {
        NavigationManager.NavigateTo("/game3");
    }


    public void Thanks()
    {
        NavigationManager.NavigateTo("/thanks");
    }

}