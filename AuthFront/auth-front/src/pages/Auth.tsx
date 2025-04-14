import { useState } from "react";
import SignIn from "../components/SignIn";
import SignUp from "../components/SignUp";

const Auth = () => {
  const [isSignUp, setIsSignUp] = useState(false);

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100 dark:bg-gray-900">
      <div className="w-full max-w-md p-6 bg-white dark:bg-gray-800 shadow-md rounded-lg">
        {isSignUp ? (
          <SignUp onSwitch={() => setIsSignUp(false)} />
        ) : (
          <SignIn onSwitch={() => setIsSignUp(true)} />
        )}
      </div>
    </div>
  );
};

export default Auth;
