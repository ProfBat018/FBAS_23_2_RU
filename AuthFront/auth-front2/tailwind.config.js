/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"], // Указываем пути к файлам
  darkMode: "class", // Включаем поддержку тёмной темы через класс "dark"
  theme: {
    extend: {
      colors: {
        primary: "#6366F1", // Фиолетовый цвет
        background: "#1E1E2E", // Фон тёмной темы
        text: "#EDEDED", // Текст в тёмной теме
      },
    },
  },
  plugins: [],
};
