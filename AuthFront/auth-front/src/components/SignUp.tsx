import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";

const registerSchema = z
  .object({
    Username: z.string().min(3, "Имя пользователя слишком короткое"),
    Email: z.string().email("Неверный email"),
    Password: z.string().min(6, "Пароль должен быть минимум 6 символов"),
    ConfirmPassword: z.string(),
  })
  .refine((data) => data.Password === data.ConfirmPassword, {
    message: "Пароли не совпадают",
    path: ["confirmPassword"],
  });

type RegisterForm = z.infer<typeof registerSchema>;

const SignUp = ({ onSwitch }: { onSwitch: () => void }) => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterForm>({ resolver: zodResolver(registerSchema) });

  const onSubmit = async (data: RegisterForm) => {
    try {
      const response = await fetch(
        "http://localhost:5293/api/v1/Account/Register",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(data),
        }
      );
      if (response.ok) {
        alert("Регистрация успешна!");
        onSwitch(); // Переключение на страницу входа
      } else {
        alert("Ошибка регистрации!");
      }
    } catch (error) {
      alert(`Ошибка сети: ${error}`);
    }
  };

  return (
    <div>
      <h2 className="text-2xl font-semibold text-gray-900 dark:text-white mb-4">
        Регистрация
      </h2>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <label className="block text-gray-700 dark:text-gray-300">
            Имя пользователя
          </label>
          <input
            {...register("Username")}
            className="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          {errors.Username && (
            <p className="text-red-500 text-sm">{errors.Username.message}</p>
          )}
        </div>

        <div>
          <label className="block text-gray-700 dark:text-gray-300">
            Email
          </label>
          <input
            {...register("Email")}
            type="email"
            className="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          {errors.Email && (
            <p className="text-red-500 text-sm">{errors.Email.message}</p>
          )}
        </div>

        <div>
          <label className="block text-gray-700 dark:text-gray-300">
            Пароль
          </label>
          <input
            {...register("Password")}
            type="password"
            className="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          {errors.Password && (
            <p className="text-red-500 text-sm">{errors.Password.message}</p>
          )}
        </div>

        <div>
          <label className="block text-gray-700 dark:text-gray-300">
            Подтвердите пароль
          </label>
          <input
            {...register("ConfirmPassword")}
            type="password"
            className="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          {errors.ConfirmPassword && (
            <p className="text-red-500 text-sm">
              {errors.ConfirmPassword.message}
            </p>
          )}
        </div>

        <button
          type="submit"
          disabled={isSubmitting}
          className="w-full bg-green-500 hover:bg-green-600 text-white py-2 rounded"
        >
          Зарегистрироваться
        </button>
      </form>

      <p className="text-sm mt-4 text-gray-600 dark:text-gray-300">
        Уже есть аккаунт?{" "}
        <button onClick={onSwitch} className="text-blue-500 hover:underline">
          Войти
        </button>
      </p>
    </div>
  );
};

export default SignUp;
