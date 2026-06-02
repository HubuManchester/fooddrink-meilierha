# FoodDrinkApp 

## Project Introduction

FoodDrinkApp is a food and drink management application based on .NET MAUI. Users can record meals, manage recipes, take food photos, and enhance the user experience through various hardware features.

---

## Main Features

### 📱 Core Features

| Feature | Description |
|---------|-------------|
| Food List | Card-style display of all foods, tap to view details |
| Smart Search | Supports keyword and semantic search (e.g., typing `spicy` finds all spicy foods) |
| Add Food | Fill in name, category, steps, supports taking photos |
| Edit Food | Modify food information, supports re-taking photos |
| Delete Food | Swipe left on card to show circular delete/edit buttons |
| Pull to Refresh | Reload the food list |

### 🛠️ Hardware Features

| Feature | Description |
|---------|-------------|
| GPS Location | Get current location and display address and coordinates |
| Shake to Recommend | Randomly recommend a food |
| Camera | Take food photos and save as Base64 |
| Text-to-Speech (TTS) | Read food name and steps aloud |
| Vibration Feedback | Vibrate on operation errors |
| Haptic Feedback | Tactile feedback on button clicks |

### 🎨 UI/UX Features

| Feature | Description |
|---------|-------------|
| Dark/Light Theme | Manual switch, automatically saves preference |
| Large Text Mode | Accessibility design, one-tap text enlargement |
| Screen Reader Support | Accessibility adaptation |
| Splash Screen | Show logo on app launch |
| Swipe to Delete/Edit | Swipe left on card to show circular action buttons |

### 💾 Data Storage

| Storage Method | Purpose |
|----------------|---------|
| Preferences | Save theme settings and font size mode (persist after app restart) |
| In-Memory Collection | Temporary food data storage (17 sample items) |

---

## Project Structure
FoodDrinkApp/
├── Models/
│ └── FoodItem.cs # Food data model
├── Pages/
│ ├── MainPage.xaml # Home page (food list)
│ ├── MainPage.xaml.cs
│ ├── AddFoodPage.xaml # Add food page
│ ├── AddFoodPage.xaml.cs
│ ├── EditFoodPage.xaml # Edit food page
│ ├── EditFoodPage.xaml.cs
│ ├── FoodDetailPage.xaml # Food detail page
│ ├── FoodDetailPage.xaml.cs
│ ├── SettingsPage.xaml # Settings page
│ ├── SettingsPage.xaml.cs
│ └── SplashPage.xaml # Splash screen
├── Services/
│ ├── FoodService.cs # Food data service
│ ├── FoodTransferService.cs # Page-to-page data transfer
│ ├── SpeechService.cs # TTS speech service
│ └── AccessibilityService.cs # Accessibility service (large text, theme)
├── ViewModels/
│ ├── BaseViewModel.cs # ViewModel base class
│ ├── FoodsViewModel.cs # Home page ViewModel
│ ├── AddFoodViewModel.cs # Add page ViewModel
│ └── FoodDetailViewModel.cs # Detail page ViewModel
├── Resources/ # Resource files
│ ├── Images/ # Image resources
│ ├── Styles/ # Style files
│ └── Fonts/ # Font files
└── Platforms/ # Platform-specific code
└── Android/
└── AndroidManifest.xml # Android permissions


---

## Smart Search Guide

| Keyword | Search Results |
|---------|----------------|
| `spicy` | Mapo Tofu, Spicy Beef Noodle Soup |
| `hot` | Hot and Sour Soup, Grilled Salmon, Roasted Chicken |
| `cold` | Iced Milk Tea, Cold Sesame Noodles, Frozen Yogurt, Mango Sorbet |
| `sweet` | Chocolate Cake, Tiramisu, Honey Glazed Donut |
| `fast` | Cheeseburger, Chicken Wrap |
| `healthy` | Quinoa Salad Bowl, Greek Yogurt Parfait |
| `grill` | Grilled Salmon, Cheeseburger |
| `soup` | Hot and Sour Soup, Spicy Beef Noodle Soup |

---

## Tech Stack

| Technology | Description |
|------------|-------------|
| .NET MAUI | Cross-platform UI framework |
| C# | Primary programming language |
| CommunityToolkit.Mvvm | MVVM toolkit (8.4.2) |
| XAML | UI definition language |
| Microsoft.Maui.Essentials | Hardware features (GPS, sensors, etc.) |

---

## How to Run

### Requirements
- Visual Studio 2022
- .NET 8.0 SDK
- MAUI workload

### Steps

 **Clone the repository**
   ```bash
   git clone https://github.com/HubuManchester/fooddrink-meilierha.git


## Permissions

The app requires the following permissions (configured in `AndroidManifest.xml`):

| Permission | Purpose |
|------------|---------|
| `CAMERA` | Take food photos |
| `ACCESS_FINE_LOCATION` | Get current location |
| `ACCESS_COARSE_LOCATION` | Approximate location |
| `VIBRATE` | Vibration feedback |
| `INTERNET` | Network access |

---

## Sample Data

The app includes 17 built-in sample foods:

| ID | Name | Category |
|----|------|----------|
| 1 | Tomato Egg Stir Fry | Home Cooking |
| 2 | Iced Milk Tea | Drink |
| 3 | Chocolate Cake | Dessert |
| 4 | Spicy Mapo Tofu | Home Cooking |
| 5 | Spicy Beef Noodle Soup | Dinner |
| 6 | Hot and Sour Soup | Dinner |
| 7 | Grilled Salmon Steak | Dinner |
| 8 | Roasted Chicken Leg | Lunch |
| 9 | Cold Sesame Noodles | Lunch |
| 10 | Frozen Yogurt | Dessert |
| 11 | Mango Sorbet | Dessert |
| 12 | Tiramisu | Dessert |
| 13 | Honey Glazed Donut | Breakfast |
| 14 | Cheeseburger | Fast Food |
| 15 | Chicken Wrap | Fast Food |
| 16 | Quinoa Salad Bowl | Healthy |
| 17 | Greek Yogurt Parfait | Breakfast |