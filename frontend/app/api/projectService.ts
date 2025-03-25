
import type { ProjectCreateDTO, ProjectDTO } from "~/models/project";
import { apiService } from "./apiService";


export const projectService = {
  fetchProject: async (projectId: string): Promise<ProjectDTO> => {
    return await apiService.get<ProjectDTO>(`/project/${projectId}`);
  },

  fetchProjectsByFreelancerId: async (freelancerId: string): Promise<ProjectDTO[]> => {
    return await apiService.get<ProjectDTO[]>(`/project/freelancer/${freelancerId}`);
  },

  createProject: async (projectData: ProjectCreateDTO): Promise<void> => {
    return await apiService.post('/project', projectData);
  },

  updateProject: async (projectId: string, updatedProject: ProjectDTO): Promise<void> => {
    return await apiService.put(`/project/${projectId}`, updatedProject);
  },

  deleteProject: async (projectId: string): Promise<void> => {
    return await apiService.delete(`/project/${projectId}`);
  },
};
