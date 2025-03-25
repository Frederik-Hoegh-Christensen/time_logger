import React, { useState } from "react";
import DayCard from "./dayCard";
import styles from "../registerTime.module.css";
import { useTimeRegistrationContext } from "~/contexts/timeRegistrationContext";
import { getMondayOfWeek, getWeekNumber } from "~/utils/dateHelper";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronCircleLeft, faChevronCircleRight } from "@fortawesome/free-solid-svg-icons";

export default function WeekView() {
  const { selectedDate, setSelectedDate, setTimeRegistrations } = useTimeRegistrationContext();
  
  // State for tracking the current week's Monday
  const [currentMonday, setCurrentMonday] = useState(getMondayOfWeek(new Date()));

  // Function to generate dates for the current week
  const getWeekDates = (monday: Date): Date[] => {
    return Array.from({ length: 7 }, (_, i) => {
      const date = new Date(monday);
      date.setUTCDate(monday.getUTCDate() + i);
      return date;
    });
  };

  // Navigate to previous week
  const goToPreviousWeek = () => {
    const prevMonday = new Date(currentMonday);
    prevMonday.setUTCDate(currentMonday.getUTCDate() - 7);
    setCurrentMonday(prevMonday);
    setSelectedDate(undefined);
    setTimeRegistrations([])
  };

  // Navigate to next week
  const goToNextWeek = () => {
    const nextMonday = new Date(currentMonday);
    nextMonday.setUTCDate(currentMonday.getUTCDate() + 7);
    setCurrentMonday(nextMonday);
    setSelectedDate(undefined);
    setTimeRegistrations([])
  };

  return (
    <div className={styles.weekViewWrapper}>
      {/* Week Navigation Header */}
      <div className={styles.weekHeader}>
        <button onClick={goToPreviousWeek}>
          <FontAwesomeIcon icon={faChevronCircleLeft} size="2x" />
        </button>
        <span>Week {getWeekNumber(currentMonday)}</span>
        <button onClick={goToNextWeek}>
          <FontAwesomeIcon icon={faChevronCircleRight} size="2x" />
        </button>
      </div>
  
      {/* Week Days */}
      <div className={styles.weekViewContainer}>
        {getWeekDates(currentMonday).map((date) => (
          <DayCard
            key={date.toISOString()}
            date={date}
            isSelected={selectedDate?.toDateString() === date.toDateString()}
            onSelect={setSelectedDate}
          />
        ))}
      </div>
    </div>
  );
}

