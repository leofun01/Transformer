# Transformer

<h2 id="contents">Contents</h2>

- [Description](#description)
- [Directory structure](#directory-structure)

<h2 id="description">Description</h2>

The main idea of project "Transformer" is
using group theory in programming
to achieve the most efficient solutions.

<h2 id="directory-structure">Directory structure</h2>

<ul>
	<li>
		<details>
			<summary><code>docs</code></summary>
			<ul>
				<li>
					<details open="open">
						<summary><code>svg</code></summary>
						<ul>
							<li>
								<details open="open">
									<summary><code>OctahedralGroup</code></summary>
									<ul>
										<li><a href="docs/svg/OctahedralGroup/CayleyGraph_2e_6af_4b_440x440.svg"><code>CayleyGraph_2e_6af_4b_440x440.svg</code></a></li>
										<li><a href="docs/svg/OctahedralGroup/CayleyGraph_3e_2f_3a_4b_440x440.svg"><code>CayleyGraph_3e_2f_3a_4b_440x440.svg</code></a></li>
									</ul>
								</details>
							</li>
							<li>
								<details open="open">
									<summary><code>TetrahedralGroup</code></summary>
									<ul>
										<li><a href="docs/svg/TetrahedralGroup/CayleyGraph_2e_3a_4b_440x440.svg"><code>CayleyGraph_2e_3a_4b_440x440.svg</code></a></li>
									</ul>
								</details>
							</li>
						</ul>
					</details>
				</li>
				<li>
					<details open="open">
						<summary><code>txt</code></summary>
						<ul>
							<li><a href="docs/txt/Tesseract_16_v1.txt"><code>Tesseract_16_v1.txt</code></a></li>
							<li><a href="docs/txt/TesseractResearchResult.txt"><code>TesseractResearchResult.txt</code></a></li>
						</ul>
					</details>
				</li>
			</ul>
		</details>
	</li>
	<li>
		<details open="open">
			<summary><code>DotNetTransformer</code></summary>
			<ul>
				<li>
					<details>
						<summary><code>Collections</code></summary>
						<ul>
							<li><a href="DotNetTransformer/Collections/EnumerableConverter.cs"><code>EnumerableConverter.cs</code></a></li>
						</ul>
					</details>
				</li>
				<li>
					<details open="open">
						<summary><code>Extensions</code></summary>
						<ul>
							<li><a href="DotNetTransformer/Extensions/ArrayExtension.cs"><code>ArrayExtension.cs</code></a></li>
							<li><a href="DotNetTransformer/Extensions/Delegate.cs"><code>Delegate.cs</code></a></li>
							<li><a href="DotNetTransformer/Extensions/EnumerableExtension.cs"><code>EnumerableExtension.cs</code></a></li>
							<li><a href="DotNetTransformer/Extensions/RotateFlipTypeExtension.cs"><code>RotateFlipTypeExtension.cs</code></a></li>
						</ul>
					</details>
				</li>
				<li>
					<details open="open">
						<summary><code>Math</code></summary>
						<ul>
							<li>
								<details open="open">
									<summary><code>Group</code></summary>
									<ul>
										<li><a href="DotNetTransformer/Math/Group/FiniteGroup.cs"><code>FiniteGroup.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Group/FiniteGroupExtension.cs"><code>FiniteGroupExtension.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Group/GroupExtension.cs"><code>GroupExtension.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Group/IFiniteGroupElement.cs"><code>IFiniteGroupElement.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Group/IGroup.cs"><code>IGroup.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Group/IGroupElement.cs"><code>IGroupElement.cs</code></a></li>
									</ul>
								</details>
							</li>
							<li>
								<details open="open">
									<summary><code>Permutation</code></summary>
									<ul>
										<li><a href="DotNetTransformer/Math/Permutation/IPermutation.cs"><code>IPermutation.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Permutation/PermutationByte.cs"><code>PermutationByte.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Permutation/PermutationExtension.cs"><code>PermutationExtension.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Permutation/PermutationInt32.cs"><code>PermutationInt32.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Permutation/PermutationInt64.cs"><code>PermutationInt64.cs</code></a></li>
									</ul>
								</details>
							</li>
							<li>
								<details open="open">
									<summary><code>Set</code></summary>
									<ul>
										<li><a href="DotNetTransformer/Math/Set/EditableFiniteSet.cs"><code>EditableFiniteSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/EmptySet.cs"><code>EmptySet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/FiniteSet.cs"><code>FiniteSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/FiniteSetExtension.cs"><code>FiniteSetExtension.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/IEditableSet.cs"><code>IEditableSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/IFiniteSet.cs"><code>IFiniteSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/ISet.cs"><code>ISet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/ISubSet.cs"><code>ISubSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/ISuperSet.cs"><code>ISuperSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/SetExtension.cs"><code>SetExtension.cs</code></a></li>
									</ul>
								</details>
							</li>
							<li>
								<details open="open">
									<summary><code>Transform</code></summary>
									<ul>
										<li><a href="DotNetTransformer/Math/Transform/FlipRotate16D.cs"><code>FlipRotate16D.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Transform/FlipRotate2D.cs"><code>FlipRotate2D.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Transform/FlipRotate3D.cs"><code>FlipRotate3D.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Transform/FlipRotate4D.cs"><code>FlipRotate4D.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Transform/FlipRotate8D.cs"><code>FlipRotate8D.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Transform/FlipRotateExtension.cs"><code>FlipRotateExtension.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Transform/IFlipRotate.cs"><code>IFlipRotate.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Transform/Polygon120.cs"><code>Polygon120.cs</code></a></li>
									</ul>
								</details>
							</li>
						</ul>
					</details>
				</li>
				<li>
					<details>
						<summary><code>Properties</code></summary>
						<ul>
							<li><a href="DotNetTransformer/Properties/AssemblyInfo.cs"><code>AssemblyInfo.cs</code></a></li>
						</ul>
					</details>
				</li>
				<li>
					<details>
						<summary><code>Test</code></summary>
						<ul>
							<li>
								<details>
									<summary><code>Properties</code></summary>
									<ul>
										<li><a href="DotNetTransformer/Test/Properties/AssemblyInfo.cs"><code>AssemblyInfo.cs</code></a></li>
									</ul>
								</details>
							</li>
							<li>
								<details>
									<summary><code>*.csproj</code></summary>
									<ul>
										<li><a href="DotNetTransformer/Test/Test_vs2008.csproj"><code>Test_vs2008.csproj</code></a></li>
										<li><a href="DotNetTransformer/Test/Test_vs2010.csproj"><code>Test_vs2010.csproj</code></a></li>
										<li><a href="DotNetTransformer/Test/Test_vs2012.csproj"><code>Test_vs2012.csproj</code></a></li>
										<li><a href="DotNetTransformer/Test/Test_vs2013.csproj"><code>Test_vs2013.csproj</code></a></li>
										<li><a href="DotNetTransformer/Test/Test_vs2015.csproj"><code>Test_vs2015.csproj</code></a></li>
										<li><a href="DotNetTransformer/Test/Test_vs2017.csproj"><code>Test_vs2017.csproj</code></a></li>
									</ul>
								</details>
							</li>
						</ul>
					</details>
				</li>
				<li><a href="DotNetTransformer/Array1DTransformer.cs"><code>Array1DTransformer.cs</code></a></li>
				<li><a href="DotNetTransformer/Array2DTransformer.cs"><code>Array2DTransformer.cs</code></a></li>
				<li>
					<details>
						<summary><code>*.csproj</code></summary>
						<ul>
							<li><a href="DotNetTransformer/DotNetTransformer_vs2008.csproj"><code>DotNetTransformer_vs2008.csproj</code></a></li>
							<li><a href="DotNetTransformer/DotNetTransformer_vs2010.csproj"><code>DotNetTransformer_vs2010.csproj</code></a></li>
							<li><a href="DotNetTransformer/DotNetTransformer_vs2012.csproj"><code>DotNetTransformer_vs2012.csproj</code></a></li>
							<li><a href="DotNetTransformer/DotNetTransformer_vs2013.csproj"><code>DotNetTransformer_vs2013.csproj</code></a></li>
							<li><a href="DotNetTransformer/DotNetTransformer_vs2015.csproj"><code>DotNetTransformer_vs2015.csproj</code></a></li>
							<li><a href="DotNetTransformer/DotNetTransformer_vs2017.csproj"><code>DotNetTransformer_vs2017.csproj</code></a></li>
						</ul>
					</details>
				</li>
			</ul>
		</details>
	</li>
	<li>
		<details>
			<summary><code>External</code></summary>
			<ul>
				<li>
					<details open="open">
						<summary><code>System.Core</code></summary>
						<ul>
							<li><a href="External/System.Core/ExtensionAttribute.cs"><code>ExtensionAttribute.cs</code></a></li>
						</ul>
					</details>
				</li>
			</ul>
		</details>
	</li>
	<li>
		<details>
			<summary><code>VS</code></summary>
			<ul>
				<li><a href="VS/Transformer_vs2008.sln"><code>Transformer_vs2008.sln</code></a></li>
				<li><a href="VS/Transformer_vs2010.sln"><code>Transformer_vs2010.sln</code></a></li>
				<li><a href="VS/Transformer_vs2012.sln"><code>Transformer_vs2012.sln</code></a></li>
				<li><a href="VS/Transformer_vs2013.sln"><code>Transformer_vs2013.sln</code></a></li>
				<li><a href="VS/Transformer_vs2015.sln"><code>Transformer_vs2015.sln</code></a></li>
				<li><a href="VS/Transformer_vs2017.sln"><code>Transformer_vs2017.sln</code></a></li>
			</ul>
		</details>
	</li>
</ul>
