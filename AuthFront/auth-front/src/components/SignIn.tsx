import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";

const loginSchema = z.object({
  username: z.string().min(3, "Имя пользователя слишком короткое"),
  password: z.string().min(6, "Пароль должен быть минимум 6 символов"),
});

type LoginForm = z.infer<typeof loginSchema>;

const SignIn = ({ onSwitch }: { onSwitch: () => void }) => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginForm>({ resolver: zodResolver(loginSchema) });

  const onSubmit = async (data: LoginForm) => {
    try {
      const response = await fetch("http://localhost:5293/api/v1/Auth/Login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });
      if (response.ok) {
        response.json().then((res) => {
          console.log(res);
          localStorage.setItem("accessToken", res.data.accessToken);
          localStorage.setItem("refreshToken", res.data.refreshToken);
        });

        // localStorage.setItem("token", response.);
        alert("Успешный вход!");
      } else {
        alert("Ошибка входа!");
      }
    } catch (error) {
      alert(`Ошибка сети: ${error}`);
    }
  };

  return (
    <div>
      <h2 className="text-2xl font-semibold text-gray-900 dark:text-white mb-4">
        Вход
      </h2>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <label className="block text-gray-700 dark:text-gray-300">
            Имя пользователя
          </label>
          <input
            {...register("username")}
            className="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          {errors.username && (
            <p className="text-red-500 text-sm">{errors.username.message}</p>
          )}
        </div>

        <div>
          <label className="block text-gray-700 dark:text-gray-300">
            Пароль
          </label>
          <input
            {...register("password")}
            type="password"
            className="w-full p-2 border rounded dark:bg-gray-700 dark:border-gray-600 dark:text-white"
          />
          {errors.password && (
            <p className="text-red-500 text-sm">{errors.password.message}</p>
          )}
        </div>

        <button
          type="submit"
          disabled={isSubmitting}
          className="w-full bg-blue-500 hover:bg-blue-600 text-white py-2 rounded"
        >
          Войти
        </button>
      </form>

      <p className="text-sm mt-4 text-gray-600 dark:text-gray-300">
        Нет аккаунта?{" "}
        <button onClick={onSwitch} className="text-blue-500 hover:underline">
          Зарегистрироваться
        </button>
      </p>
    </div>
  );
};

export default SignIn;
