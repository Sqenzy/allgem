using System;

namespace Allgem01.Logic;

public class Navigation
{
    public void Home()
    {

        if (Repository.SignedIn == true)
        {
            NavigationManager.NavigateTo("/home");    
        }
        else
        {
            NavigationManager.NavigateTo("/");
        }

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
        NavigationManager.NavigateTo("/thanks")
    }

}