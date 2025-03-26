import type { TimeRegistrationCreateDTO, TimeRegistrationDTO } from "~/models/timeRegistration";
import { apiService } from "./apiService";

export const timeRegistrationService = {
  // Fetch Time Registration by ID
  fetchTimeRegistration: async (timeRegistrationId: string): Promise<TimeRegistrationDTO> => {
    return await apiService.get<TimeRegistrationDTO>(`/timeRegistrations/${timeRegistrationId}`);
  },

  // Fetch Time Registrations by Project ID
  fetchTimeRegistrationsByProjectId: async (projectId: string): Promise<TimeRegistrationDTO[]> => {
    return await apiService.get<TimeRegistrationDTO[]>(`/timeRegistrations/project/${projectId}`);
  },

  // Fetch Time Registrations by Freelancer ID and Date
  

  // Create a new Time Registration
  createTimeRegistration: async (timeRegistrationData: TimeRegistrationCreateDTO): Promise<void> => {
    return await apiService.post('/timeRegistrations', timeRegistrationData);
  },

  // Update an existing Time Registration
  updateTimeRegistration: async (timeRegistrationId: string, updatedTimeRegistration: TimeRegistrationDTO): Promise<void> => {
    return await apiService.put(`/timeRegistrations/${timeRegistrationId}`, updatedTimeRegistration);
  },

  // Delete a Time Registration by ID
  deleteTimeRegistration: async (timeRegistrationId: string): Promise<void> => {
    return await apiService.delete(`/timeRegistrations/${timeRegistrationId}`);
  },
};
