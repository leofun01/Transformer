# Transformer

## Contents

- [Description](#description)
- [Directory structure](#directory-structure)

## Description

The main idea of project "Transformer" is
using group theory in programming
to achieve the most efficient solutions.

## Directory structure

<ul>
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
										<li>
											<details open="open">
												<summary><code>Permutation</code></summary>
												<ul>
													<li><a href="DotNetTransformer/Math/Group/Permutation/IPermutation.cs"><code>IPermutation.cs</code></a></li>
													<li><a href="DotNetTransformer/Math/Group/Permutation/PermutationByte.cs"><code>PermutationByte.cs</code></a></li>
													<li><a href="DotNetTransformer/Math/Group/Permutation/PermutationExtension.cs"><code>PermutationExtension.cs</code></a></li>
													<li><a href="DotNetTransformer/Math/Group/Permutation/PermutationInt32.cs"><code>PermutationInt32.cs</code></a></li>
													<li><a href="DotNetTransformer/Math/Group/Permutation/PermutationInt64.cs"><code>PermutationInt64.cs</code></a></li>
												</ul>
											</details>
										</li>
										<li>
											<details open="open">
												<summary><code>Transform2D</code></summary>
												<ul>
													<li><a href="DotNetTransformer/Math/Group/Transform2D/FlipRotate2D.cs"><code>FlipRotate2D.cs</code></a></li>
													<li><a href="DotNetTransformer/Math/Group/Transform2D/Polygon120.cs"><code>Polygon120.cs</code></a></li>
												</ul>
											</details>
										</li>
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
									<summary><code>Set</code></summary>
									<ul>
										<li><a href="DotNetTransformer/Math/Set/EditableFiniteSet.cs"><code>EditableFiniteSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/EmptySet.cs"><code>EmptySet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/FiniteSet.cs"><code>FiniteSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/IEditableSet.cs"><code>IEditableSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/ISet.cs"><code>ISet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/ISubSet.cs"><code>ISubSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/ISuperSet.cs"><code>ISuperSet.cs</code></a></li>
										<li><a href="DotNetTransformer/Math/Set/SetExtension.cs"><code>SetExtension.cs</code></a></li>
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
	<li>
		<details>
			<summary><code>svg</code></summary>
			<ul>
				<li>
					<details>
						<summary><code>OctahedralGroup</code></summary>
						<ul>
							<li><a href="svg/OctahedralGroup/CayleyGraph_2e_6af_4b_440x440.svg"><code>CayleyGraph_2e_6af_4b_440x440.svg</code></a></li>
							<li><a href="svg/OctahedralGroup/CayleyGraph_3e_2f_3a_4b_440x440.svg"><code>CayleyGraph_3e_2f_3a_4b_440x440.svg</code></a></li>
						</ul>
					</details>
				</li>
				<li>
					<details>
						<summary><code>TetrahedralGroup</code></summary>
						<ul>
							<li><a href="svg/TetrahedralGroup/CayleyGraph_2e_3a_4b_440x440.svg"><code>CayleyGraph_2e_3a_4b_440x440.svg</code></a></li>
						</ul>
					</details>
				</li>
			</ul>
		</details>
	</li>
</ul>
