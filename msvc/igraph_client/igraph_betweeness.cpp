//--------------------------------------------DLL-------------------------------------------
///* -*- mode: C -*-  */
///* 

//   IGraph library.
//   Copyright (C) 2006-2012  Gabor Csardi <csardi.gabor@gmail.com>
//   334 Harvard street, Cambridge, MA 02139 USA
//   
//   This program is free software; you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation; either version 2 of the License, or
//   (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License
//   along with this program; if not, write to the Free Software
//   Foundation, Inc.,  51 Franklin Street, Fifth Floor, Boston, MA 
//   02110-1301 USA
//
//*/
////#include "stdafx.h"
//#include <igraph.h>
//
//void print_vector(igraph_vector_t *v, FILE *f) {
//  long int i;
//  for (i=0; i<igraph_vector_size(v); i++) {
//    fprintf(f, " %li", (long int) VECTOR(*v)[i]);
//  }
//  fprintf(f, "\n");
//}
//
//--------------------------------------------DLL-------------------------------------------
#include "igraph.h"

#include <Windows.h>
extern "C"
{
	__declspec(dllexport) void memory_release(int* pArray)
	{
		delete[] pArray;
	}

	__declspec(dllexport) double* betweenness(int param[1000], int wei[500], int size_vectorsPairs)
		//double* betweenness()
		//double* main(int param[1000], int size_nodes, int size_vectorsPairs)
	{


		//int biggest = findMax(param, arraySize);

		igraph_t g;
		igraph_vector_t bet, weights, edges; // bet2;

		igraph_vector_t v1;

		//igraph_vector_t	v2;
		igraph_vector_init(&v1, size_vectorsPairs * 2);
		for (int i = 0; i < size_vectorsPairs * 2; i++)
		{
			VECTOR(v1)[i] = param[i];
		}

		igraph_vector_t w1;
		igraph_vector_init(&w1, size_vectorsPairs);
		for (int i = 0; i < size_vectorsPairs; i++)
		{
			VECTOR(w1)[i] = wei[i];
		}

		//VECTOR(v1)[0] = 0; VECTOR(v1)[1] = 1;
		//VECTOR(v1)[2] = 1; VECTOR(v1)[3] = 2;
		//VECTOR(v1)[4] = 2; VECTOR(v1)[5] = 3;
		//VECTOR(v1)[6] = 2; VECTOR(v1)[7] = 2;



		//for (int i = 0; i < igraph_vector_size(&v1); i++)
		//{
		//	fprintf(stdout, "%.5f\n", (float)VECTOR(v1)[i]);
		//}	
		//graph_create(&g, &v1, 0, 0);
		//igraph_vector_init(&v2, 0);
		//igraph_get_edgelist(&g, &v2, 0);
		//for (int i = 0; i < igraph_vector_size(&v2); i++)
		//{
		//	fprintf(stdout, "%.5f\n", (float)VECTOR(v2)[i]);
		//}

		//--------
		igraph_real_t nontriv[] = { 1, 2, 1, 3, 2, 3, 3, 4, 3, 5, 2, 5, 5, 7, 5, 6 };

		//	0, 1, 1, 2, 2, 3, 3, 4, 6, 1

		// { 1, 2, 1, 3, 2, 3, 3, 4, 3, 5, 2, 5, 5, 7, 5, 6 };

		//int biggest = findMax(nontriv, size_vectorsPairs);


		igraph_real_t nontriv_weights[] = { 1,1,1,1,1,2,1,1, };

		igraph_real_t nontriv_res[] = { 20, 0, 0, 0, 0, 19, 80, 85, 32, 0, 10,
			75, 70, 0, 36, 81, 60, 0, 19, 19, 86 };

		igraph_tree(&g, 20000, 10, IGRAPH_TREE_UNDIRECTED);

		//igraph_vector_init(&bet, 0);

		//igraph_betweenness_estimate(/* graph=     */ &g,
		//	/* res=       */ &bet,
		//	/* vids=      */ igraph_vss_all(),
		//	/* directed = */ 0,
		//	/* cutoff=    */ 3,
		//	/* weights=   */ 0,
		//	/* nobigint=  */ 1);

		//igraph_vector_init(&bet2, 0);
		//igraph_vector_init(&weights, igraph_ecount(&g));
		//igraph_vector_fill(&weights, 1.0);

		//igraph_betweenness_estimate(/* graph=     */ &g,
		//	/* res=       */ &bet2,
		//	/* vids=      */ igraph_vss_all(),
		//	/* directed = */ 0,
		//	/* cutoff=    */ 3,
		//	/* weights=   */ &weights,
		//	/* nobigint=  */ 1);

		////if (!igraph_vector_all_e(&bet, &bet2)) {
		////	return 1;
		////}

		//igraph_vector_destroy(&bet);
		//igraph_vector_destroy(&bet2);
		//igraph_vector_destroy(&weights);
		//igraph_destroy(&g);

		/* Non-trivial weighted graph */
		//igraph_vector_view(&edges, nontriv, sizeof(nontriv) / sizeof(igraph_real_t));
		//for (int i = 0; i < igraph_vector_size(&edges); i++)
		//{
		//	fprintf(stdout, "%.5f\n", (float)VECTOR(edges)[i]);
		//}

		//igraph_create(&g, &edges, 0, /* undirected= */ 1);
		//for (int i = 0; i < igraph_vector_size(&edges); i++)
		//{
		//	fprintf(stdout, "%.5f\n", (float)VECTOR(edges)[i]);
		//}

		//fprintf(stdout, "\n");

		igraph_create(&g, &v1, 0, 1);
		//for (int i = 0; i < igraph_vector_size(&v1); i++)
		//{
		//	fprintf(stdout, "%.5f\n", (float)VECTOR(v1)[i]);
		//}

		igraph_vector_view(&weights, nontriv_weights, sizeof(nontriv_weights) / sizeof(igraph_real_t));
		igraph_vector_init(&bet, 0);

		//for (int i = 0; i < igraph_vector_size(&weights); i++)
		//{
		//	fprintf(stdout, "%.5f\n", (float)VECTOR(weights)[i]);
		//}
		//fprintf(stdout, "\n");
		//for (int i = 0; i < igraph_vector_size(&w1); i++)
		//{
		//	fprintf(stdout, "%.5f\n", (float)VECTOR(w1)[i]);
		//}

		//igraph_betweenness(/*graph=*/ &g, /*res=*/ &bet, /*vids=*/ igraph_vss_all(),
		//	/*directed=*/0, /*weights=*/ &weights, /*nobigint=*/ 1);

		igraph_betweenness(/*graph=*/ &g, /*res=*/ &bet, /*vids=*/ igraph_vss_all(),
			/*directed=*/0, /*weights=*/ &w1, /*nobigint=*/ 1);


		double* result = new double[igraph_vector_size(&bet)];

		for (int i = 0; i < igraph_vector_size(&bet); i++)
		{
			fprintf(stdout, "%.5f\n", (double)VECTOR(bet)[i]);
			result[i] = (double)VECTOR(bet)[i];
		}

		return result;
	}
}