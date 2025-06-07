# 🎬 Kino Reactowe – System Rezerwacji Miejsc

Aplikacja webowa umożliwiająca przeglądanie repertuaru filmowego, wybór seansu, wybór miejsca oraz złożenie rezerwacji. Projekt został stworzony w React z wykorzystaniem REST API.

## 📸 Zrzuty ekranu

| Repertuar | Seanse | Miejsca | Formularz |
|----------|--------|---------|-----------|
| ![](./screenshots/repertuar.png) | ![](./screenshots/seanse.png) | ![](./screenshots/miejsca.png) | ![](./screenshots/formularz.png) |

## 🔧 Technologie

- **Frontend:** React (JSX, React Router, Hooks)
- **Stylizacja:** CSS (responsywny layout, niestandardowy design kinowy)
- **Backend (API):** ASP.NET Core (lub inny REST API)
- **Inne:** `fetch`, komponenty dynamiczne, routing, `useNavigate`, `useParams`, `useLocation`

## 🧩 Funkcjonalności

- 🗂️ Lista aktualnych filmów z plakatem i opisem
- 🎟️ Przegląd seansów dla danego filmu
- 🪑 Wybór miejsca z wizualizacją sali kinowej
- ✍️ Formularz rezerwacji z danymi kontaktowymi
- ✅ Obsługa zajętych miejsc i ich blokada
- 📦 Komunikacja z API (`/api/Movies`, `/api/Screenings`, `/api/Seats`, `/api/Reservations`)

## 📁 Struktura projektu

src/
├── App.js
├── App.css
├── MovieDetails.js
├── ScreeningSeats.js
├── ReservationForm.js
└── components/ (opcjonalnie)


## 🚀 Jak uruchomić

1. Sklonuj repozytorium:

git clone https://github.com/TwojLogin/nazwa-projektu.git
cd nazwa-projektu


2. Zainstaluj zależności:

npm install


3. Uruchom frontend:

npm start


4. Upewnij się, że backend API działa pod `http://localhost:5000` lub zmień adres w plikach `fetch(...)`

## 💡 Możliwości rozbudowy

- Filtrowanie i sortowanie filmów
- Potwierdzenie e-mail po rezerwacji
- Panel administracyjny (CRUD dla filmów i seansów)
- Historia rezerwacji użytkownika

## 🧑‍💻 Autor

**Aleksandra Strzelczyk**

## 📄 Licencja

MIT – możesz swobodnie korzystać, modyfikować i rozwijać dalej.

