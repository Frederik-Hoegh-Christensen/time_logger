import { createContext, useContext, useState } from "react";
import type { ReactNode } from "react";

interface SomeContextType {
  value: string;
  setValue: (newValue: string) => void;
}
// Example context
const SomeContext = createContext<SomeContextType | undefined>(undefined);

export const SomeProvider = ({ children }: { children: ReactNode }) => {
  const [value, setValue] = useState("default value");

  return (
    <SomeContext.Provider value={{ value, setValue }}>
      {children}
    </SomeContext.Provider>
  );
};

export const useSomeContext = () => {
  const context = useContext(SomeContext);
  if (!context) {
    throw new Error("useSomeContext must be used within a SomeProvider");
  }
  return context;
};
