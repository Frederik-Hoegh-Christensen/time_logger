import { createContext, useContext, useState } from "react";
import type { ReactNode } from "react";
import type { ProjectDTO } from "~/models/project";
import type { TimeRegistrationDTO } from "~/models/timeRegistration";

interface TimeRegistrationContextType {
  selectedDate: Date | undefined;
  setSelectedDate: (date: Date | undefined) => void;
  timeRegistrations: TimeRegistrationDTO[];
  setTimeRegistrations: (registrations: TimeRegistrationDTO[]) => void;
}

const TimeRegistrationContext = createContext<TimeRegistrationContextType | undefined>(undefined);

export const TimeRegistrationProvider = ({ children }: { children: ReactNode }) => {
  const [selectedDate, setSelectedDate] = useState<Date | undefined>(undefined);
  const [timeRegistrations, setTimeRegistrations] = useState<TimeRegistrationDTO[]>([]);

  return (
    <TimeRegistrationContext.Provider value={{ selectedDate, setSelectedDate, timeRegistrations, setTimeRegistrations }}>
      {children}
    </TimeRegistrationContext.Provider>
  );
};

export const useTimeRegistrationContext = () => {
  const context = useContext(TimeRegistrationContext);
  if (!context) {
    throw new Error("useTimeRegistrationContext must be used within a TimeRegistrationProvider");
  }
  return context;
};
