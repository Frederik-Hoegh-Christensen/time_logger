import axiosInstance from './axiosInstance';

export const apiService = {
  // GET request
  get: async <T>(url: string, params?: Record<string, any>): Promise<T> => {
    try {
      const response = await axiosInstance.get(url, { params });
      return response.data;
    } catch (error) {
      // Handle error (optional)
      // handleError(error); 
      throw error;
    }
  },

  // POST request
  post: async <T>(url: string, data: any): Promise<T> => {
    try {
      const response = await axiosInstance.post(url, data);
      return response.data;
    } catch (error) {
      // Handle error (optional)
      // handleError(error); 
      console.log(error)
      throw error;
    }
  },

  // PUT request
  put: async <T>(url: string, data: any): Promise<T> => {
    try {
      const response = await axiosInstance.put(url, data);
      return response.data;
    } catch (error) {
      // Handle error (optional)
      // handleError(error); 
      throw error;
    }
  },

  // DELETE request
  delete: async <T>(url: string): Promise<T> => {
    try {
      const response = await axiosInstance.delete(url);
      return response.data;
    } catch (error) {
      // Handle error (optional)
      // handleError(error); 
      throw error;
    }
  },

};