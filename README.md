# Xalami
Nobody loves boilerplate, especially if feels like you rewrite it every time you start a new project. Xalami is a starting point for Xamarin Forms projects that includes a lot of elements you'll use in every project. It's also delicious!

Included in the project are such tasty goodies as:
- An emphasis on MVVM
- A simple localization framework based on .resx files
- A Navigation service which wraps a Xamarin Forms NavigationPage, support parameter passing, and navigation by View type or ViewModel type.
- Several useful XAML markup extensions
- An ItemsStack control
- And more!

## Getting Started with Xalami
Check out the [Getting Started](https://github.com/futurice/Xalami/wiki/Getting-Started) page on the wiki!

Or just dive in and [download it from the Visual Studio Marketplace](https://marketplace.visualstudio.com/vsgallery/026321a8-871e-49de-b129-196c6dad34c9).

If you're using Visual Studio, you can download it directly from inside the IDE. Go to Tools -> Extension Manager -> Online, and search for Xalami. After you install it, when you go File -> New -> Project, Xalami will be in your list under Visual C# -> Cross-Platform.

## Philosophy
Xalami is *lightweight* but *opinionated*. This means that we expect you to use the template as a starting point, and not a heavyweight framework that you must conform to. Xalami *does* however expect you to follow a few basic tenets:
- You app is based around an MVVM architecture
- You have a centralized navigation service
- You have one ViewModel per view, and a view corresponds roughly to a single "page" in your app
-  You use XAML to define your UI

## Contributing
[TBD]

## Credits
Much love to [Futurice](http://futurice.com/) for making this possible.

Just as much love to the [Pepperoni App Kit](https://github.com/futurice/pepperoni-app-kit) which served as this project's inspiration.